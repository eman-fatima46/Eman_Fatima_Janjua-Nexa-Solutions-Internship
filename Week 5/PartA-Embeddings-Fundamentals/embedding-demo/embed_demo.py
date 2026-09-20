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

    # Some providers/models may return shape (1, dimensions).
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


sentence_1 = (
    "A young wizard attends a magic school"
)

sentence_2 = (
    "A boy learns spells at an academy"
)

sentence_3 = (
    "A recipe for chocolate cake"
)


v1 = embed(sentence_1)
v2 = embed(sentence_2)
v3 = embed(sentence_3)


similar_score = cosine_similarity(
    v1,
    v2
)

unrelated_score = cosine_similarity(
    v1,
    v3
)


print("Sentence 1:")
print(sentence_1)

print("\nSentence 2:")
print(sentence_2)

print("\nSentence 3:")
print(sentence_3)


print("\n--- Similarity Results ---")

print(
    "Similar meaning:",
    similar_score
)

print(
    "Unrelated meaning:",
    unrelated_score
)


print("\n--- Embedding Information ---")

print(
    "Length of v1:",
    len(v1)
)

print(
    "First 10 values of v1:"
)

print(
    v1[:10]
)