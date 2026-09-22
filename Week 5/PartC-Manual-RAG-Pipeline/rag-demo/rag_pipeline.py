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
        "HF_TOKEN was not found in the .env file."
    )


if not GROQ_API_KEY:
    raise ValueError(
        "GROQ_API_KEY was not found in the .env file."
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


chroma_client = chromadb.Client()


collection = chroma_client.create_collection(
    name="library_rag"
)


def chunk_text(
    text: str,
    chunk_size: int = 300,
    overlap: int = 50
) -> list[str]:

    chunks = []

    start = 0

    while start < len(text):

        end = start + chunk_size

        chunk = text[start:end]

        chunks.append(
            chunk
        )

        start += (
            chunk_size - overlap
        )

    return chunks


def embed(
    text: str
) -> list[float]:

    response = (
        embedding_client
        .feature_extraction(
            text,
            model=EMBEDDING_MODEL
        )
    )

    vector = np.asarray(
        response,
        dtype=float
    )

    if vector.ndim > 1:
        vector = vector[0]

    return vector.tolist()


def add_document(
    text: str,
    source: str
):

    chunks = chunk_text(
        text
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
                "source": source,
                "chunk": index
            }
            for index in range(
                len(chunks)
            )
        ],

        ids=[
            f"{source}-{index}"
            for index in range(
                len(chunks)
            )
        ]
    )


def retrieve(
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

            max_tokens=300
        )
    )

    answer = (
        response
        .choices[0]
        .message
        .content
    )

    return (
        answer
        or "I don't have that information."
    )


def load_documents():

    documents_folder = Path(
        "documents"
    )

    for file_path in (
        documents_folder
        .glob("*.txt")
    ):

        text = file_path.read_text(
            encoding="utf-8"
        )

        add_document(
            text=text,
            source=file_path.name
        )

        print(
            f"Added document: {file_path.name}"
        )


def ask_question(
    question: str
):

    print(
        "\n=================================="
    )

    print(
        "Question:"
    )

    print(
        question
    )


    chunks, metadatas = retrieve(
        question
    )


    print(
        "\n--- Retrieved Chunks ---"
    )


    for index, chunk in enumerate(
        chunks,
        start=1
    ):

        print(
            f"\nChunk {index}:"
        )

        print(
            chunk
        )


    answer = generate_answer(
        question,
        chunks
    )


    print(
        "\n--- Final Answer ---"
    )

    print(
        answer
    )


    print(
        "\n--- Sources ---"
    )


    for metadata in metadatas:

        print(
            f"Source: {metadata['source']} "
            f"| Chunk: {metadata['chunk']}"
        )


if __name__ == "__main__":

    load_documents()

    ask_question(
        "How did Lena survive on Mars?"
    )