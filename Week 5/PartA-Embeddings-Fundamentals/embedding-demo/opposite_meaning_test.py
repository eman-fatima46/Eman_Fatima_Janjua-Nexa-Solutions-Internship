import os

import numpy as np
from dotenv import load_dotenv
from huggingface_hub import InferenceClient


load_dotenv()


token = os.getenv("HF_TOKEN")

if not token:
    raise ValueError(
        "HF_TOKEN was not found in the .env file."
    )


client = InferenceClient(
    token=token
)


MODEL = "sentence-transformers/all-MiniLM-L6-v2"


def embed(text: str) -> list[float]:

    response = client.feature_extraction(
        text,
        model=MODEL
    )

    vector = np.asarray(
        response,
        dtype=float
    )

    if vector.ndim > 1:
        vector = vector[0]

    return vector.tolist()


def cosine_similarity(
    vector_a: list[float],
    vector_b: list[float]
) -> float:

    a = np.array(
        vector_a,
        dtype=float
    )

    b = np.array(
        vector_b,
        dtype=float
    )

    return float(
        np.dot(a, b)
        /
        (
            np.linalg.norm(a)
            *
            np.linalg.norm(b)
        )
    )


sentence_positive = (
    "I loved this book"
)

sentence_negative = (
    "I did not love this book"
)


positive_vector = embed(
    sentence_positive
)

negative_vector = embed(
    sentence_negative
)


score = cosine_similarity(
    positive_vector,
    negative_vector
)


print("Sentence 1:")
print(sentence_positive)

print("\nSentence 2:")
print(sentence_negative)

print(
    "\nCosine similarity:",
    score
)