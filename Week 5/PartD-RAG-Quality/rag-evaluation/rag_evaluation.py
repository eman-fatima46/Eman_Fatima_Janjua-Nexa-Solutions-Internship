import os
from pathlib import Path

import chromadb
import numpy as np
from dotenv import load_dotenv
from groq import Groq
from huggingface_hub import InferenceClient


load_dotenv()


HF_TOKEN = os.getenv("HF_TOKEN")
GROQ_API_KEY = os.getenv("GROQ_API_KEY")


if not HF_TOKEN:
    raise ValueError(
        "HF_TOKEN was not found in .env"
    )


if not GROQ_API_KEY:
    raise ValueError(
        "GROQ_API_KEY was not found in .env"
    )


EMBEDDING_MODEL = (
    "sentence-transformers/all-MiniLM-L6-v2"
)

CHAT_MODEL = "openai/gpt-oss-20b"


embedding_client = InferenceClient(
    token=HF_TOKEN
)


groq_client = Groq(
    api_key=GROQ_API_KEY
)


TEST_QUESTIONS = [
    {
        "question":
            "How did Lena survive on Mars?",

        "expected":
            "She used scientific knowledge, "
            "grew food, repaired communication "
            "equipment, and sent a signal to Earth.",

        "expected_source":
            "mars_mission.txt"
    },

    {
        "question":
            "What subjects do students study "
            "at the Academy of Arcane Arts?",

        "expected":
            "Spell casting, potion making, "
            "magical creatures, and defensive magic.",

        "expected_source":
            "magic_school.txt"
    },

    {
        "question":
            "What threatens biodiversity "
            "in the Amazon rainforest?",

        "expected":
            "Deforestation destroys habitats "
            "and reduces biodiversity.",

        "expected_source":
            "rainforest.txt"
    },

    {
        "question":
            "What is special about the Pacific Ocean?",

        "expected":
            "It is the largest and deepest "
            "ocean on Earth.",

        "expected_source":
            "ocean.txt"
    },

    {
        "question":
            "Where is the Academy of Arcane Arts located?",

        "expected":
            "Inside an ancient castle surrounded "
            "by enchanted forests.",

        "expected_source":
            "magic_school.txt"
    }
]


def embed(
    text: str
) -> list[float]:

    response = embedding_client.feature_extraction(
        text,
        model=EMBEDDING_MODEL
    )

    vector = np.asarray(
        response,
        dtype=float
    )

    if vector.ndim > 1:
        vector = vector[0]

    return vector.tolist()


def chunk_text(
    text: str,
    chunk_size: int,
    overlap: int = 50
) -> list[str]:

    chunks = []

    start = 0

    while start < len(text):

        end = start + chunk_size

        chunks.append(
            text[start:end]
        )

        start += (
            chunk_size - overlap
        )

    return chunks


def build_collection(
    chunk_size: int
):

    chroma_client = chromadb.Client()

    collection = (
        chroma_client.create_collection(
            name=f"rag_eval_{chunk_size}"
        )
    )

    documents_folder = Path(
        "documents"
    )


    for file_path in (
        documents_folder.glob("*.txt")
    ):

        text = file_path.read_text(
            encoding="utf-8"
        )

        chunks = chunk_text(
            text,
            chunk_size=chunk_size
        )

        embeddings = [
            embed(chunk)
            for chunk in chunks
        ]

        collection.add(
            documents=chunks,

            embeddings=embeddings,

            metadatas=[
                {
                    "source": file_path.name,
                    "chunk": index
                }
                for index in range(
                    len(chunks)
                )
            ],

            ids=[
                f"{file_path.name}-{chunk_size}-{index}"
                for index in range(
                    len(chunks)
                )
            ]
        )

    return collection


def retrieve(
    collection,
    question: str,
    k: int = 3
):

    question_embedding = embed(
        question
    )

    results = collection.query(
        query_embeddings=[
            question_embedding
        ],
        n_results=k
    )

    return (
        results["documents"][0],
        results["metadatas"][0]
    )


def build_prompt(
    question: str,
    chunks: list[str]
) -> str:

    context = "\n\n".join(
        chunks
    )

    return f"""
Answer the question using ONLY the context below.

If the answer is not contained in the context,
say exactly:

I don't have that information.

Do not use outside knowledge.
Do not guess.
Do not invent facts.

Context:

{context}

Question:

{question}
"""


def generate_answer(
    question: str,
    chunks: list[str]
) -> str:

    prompt = build_prompt(
        question,
        chunks
    )

    response = (
        groq_client
        .chat
        .completions
        .create(
            model=CHAT_MODEL,

            messages=[
                {
                    "role": "user",
                    "content": prompt
                }
            ],

            temperature=0,

            max_tokens=250
        )
    )

    return (
        response
        .choices[0]
        .message
        .content
        or "I don't have that information."
    )


def run_evaluation(
    chunk_size: int
):

    print(
        "\n======================================"
    )

    print(
        f"EVALUATION WITH CHUNK SIZE = {chunk_size}"
    )

    print(
        "======================================"
    )


    collection = build_collection(
        chunk_size
    )


    for number, test in enumerate(
        TEST_QUESTIONS,
        start=1
    ):

        question = test["question"]

        expected = test["expected"]

        expected_source = (
            test["expected_source"]
        )


        chunks, metadatas = retrieve(
            collection,
            question
        )


        answer = generate_answer(
            question,
            chunks
        )


        retrieved_sources = [
            metadata["source"]
            for metadata in metadatas
        ]


        print(
            f"\nTEST QUESTION {number}"
        )

        print(
            "Question:",
            question
        )

        print(
            "Expected answer:",
            expected
        )

        print(
            "Expected source:",
            expected_source
        )

        print(
            "Retrieved sources:",
            retrieved_sources
        )

        print(
            "\nGenerated answer:"
        )

        print(
            answer
        )

        print(
            "\nManual evaluation:"
        )

        print(
            "Retrieval correct? YES / NO"
        )

        print(
            "Answer correct? YES / NO"
        )

        print(
            "Failure type if any: "
            "Retrieval / Generation / None"
        )

        print(
            "-" * 60
        )


if __name__ == "__main__":

    run_evaluation(
        chunk_size=300
    )

    run_evaluation(
        chunk_size=150
    )