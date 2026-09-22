import os
from pathlib import Path

import chromadb
import numpy as np
from dotenv import load_dotenv
from fastapi import FastAPI, HTTPException
from groq import Groq
from huggingface_hub import InferenceClient
from pydantic import BaseModel


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


app = FastAPI(
    title="Library AI Service",
    description=(
        "Week 5 FastAPI service with "
        "manual RAG-powered question answering."
    ),
    version="1.0.0"
)


class SummaryRequest(BaseModel):
    title: str
    description: str


class SummaryResponse(BaseModel):
    genre: str
    summary: str


class AskRequest(BaseModel):
    question: str


class AskResponse(BaseModel):
    answer: str
    sources: list[str]


chroma_client = chromadb.Client()


collection = chroma_client.create_collection(
    name="library_rag_api"
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

        chunks.append(
            text[start:end]
        )

        start += (
            chunk_size - overlap
        )

    return chunks


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


def load_documents():

    documents_folder = Path(
        "documents"
    )

    for file_path in documents_folder.glob(
        "*.txt"
    ):

        text = file_path.read_text(
            encoding="utf-8"
        )

        add_document(
            text=text,
            source=file_path.name
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


@app.get("/health")
def health():

    return {
        "status": "ok"
    }


@app.post(
    "/summarize",
    response_model=SummaryResponse
)
def summarize(
    req: SummaryRequest
):

    prompt = f"""
Return ONLY valid JSON.

Use this exact structure:

{{
  "genre": "string",
  "summary": "one paragraph string"
}}

Book title:
{req.title}

Book description:
{req.description}
"""

    try:

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

    except Exception as error:

        print(
            f"LLM error: {error}"
        )

        raise HTTPException(
            status_code=503,
            detail=(
                "The AI provider is currently unavailable."
            )
        )


    import json


    raw_output = (
        response
        .choices[0]
        .message
        .content
    )


    try:

        parsed = json.loads(
            raw_output
        )

    except Exception:

        raise HTTPException(
            status_code=502,
            detail=(
                "The AI provider returned invalid JSON."
            )
        )


    return SummaryResponse(
        genre=parsed["genre"],
        summary=parsed["summary"]
    )


@app.post(
    "/ask",
    response_model=AskResponse
)
def ask(
    req: AskRequest
):

    chunks, metadatas = retrieve(
        req.question
    )


    if not chunks:

        return AskResponse(
            answer=(
                "I don't have that information."
            ),
            sources=[]
        )


    prompt = build_prompt(
        req.question,
        chunks
    )


    try:

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

                max_tokens=400
            )
        )


        answer = (
            response
            .choices[0]
            .message
            .content
        )


        if not answer:

            answer = (
                "I don't have that information."
            )


    except Exception as error:

        print(
            f"LLM request failed: {error}"
        )

        raise HTTPException(
            status_code=503,
            detail=(
                "The AI provider is currently unavailable."
            )
        )


    sources = sorted(
        set(
            metadata["source"]
            for metadata in metadatas
        )
    )


    return AskResponse(
        answer=answer,
        sources=sources
    )


load_documents()