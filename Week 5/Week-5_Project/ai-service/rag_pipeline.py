import json
import os
from pathlib import Path

import chromadb
import numpy as np
from dotenv import load_dotenv
from huggingface_hub import InferenceClient


load_dotenv()


HF_TOKEN = os.getenv("HF_TOKEN")


if not HF_TOKEN:
    raise ValueError(
        "HF_TOKEN was not found in .env"
    )


EMBEDDING_MODEL = (
    "sentence-transformers/all-MiniLM-L6-v2"
)


BASE_DIR = Path(__file__).resolve().parent


CORPUS_FILE = (
    BASE_DIR
    / "data"
    / "books_corpus.json"
)


embedding_client = InferenceClient(
    token=HF_TOKEN
)


chroma_client = chromadb.Client()


collection = chroma_client.create_collection(
    name="library_catalog_rag"
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


def load_corpus() -> list[dict]:

    if not CORPUS_FILE.exists():

        raise FileNotFoundError(
            "books_corpus.json was not found. "
            "Run corpus/fetch_books.py first."
        )

    return json.loads(
        CORPUS_FILE.read_text(
            encoding="utf-8"
        )
    )


def build_vector_store() -> int:

    books = load_corpus()

    added_chunks = 0


    for book in books:

        chunks = chunk_text(
            book["text"]
        )


        embeddings = [
            embed(chunk)
            for chunk in chunks
        ]


        metadatas = [
            {
                "book_id": (
                    book["book_id"]
                    if book["book_id"] is not None
                    else 0
                ),

                "title": book["title"],

                "author": book["author"],

                "category": book["category"],

                "source": book["title"],

                "chunk": index
            }

            for index in range(
                len(chunks)
            )
        ]


        ids = [
            f"book-{book['book_id']}-chunk-{index}"

            for index in range(
                len(chunks)
            )
        ]


        collection.add(
            documents=chunks,
            embeddings=embeddings,
            metadatas=metadatas,
            ids=ids
        )


        added_chunks += len(chunks)


    return added_chunks


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
You are a library knowledge assistant.

Answer the question using ONLY the library
catalog context below.

If the answer is not contained in the context,
say exactly:

I don't have that information.

Do not use outside knowledge.
Do not guess.
Do not invent books, authors, categories,
descriptions, or facts.

Context:

{context}

Question:

{question}
"""