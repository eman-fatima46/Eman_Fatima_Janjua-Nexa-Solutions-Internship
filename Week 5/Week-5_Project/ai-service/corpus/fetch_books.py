import json
from pathlib import Path

import requests


API_URL = "http://localhost:5120/api/books"


BASE_DIR = Path(__file__).resolve().parents[1]


OUTPUT_FILE = (
    BASE_DIR
    / "data"
    / "books_corpus.json"
)


def fetch_books() -> list[dict]:

    response = requests.get(
        API_URL,
        timeout=30
    )

    response.raise_for_status()

    return response.json()


def build_corpus(
    books: list[dict]
) -> list[dict]:

    corpus = []

    for book in books:

        title = book.get(
            "title",
            "Unknown Title"
        )

        author = book.get(
            "author",
            "Unknown Author"
        )

        description = book.get(
            "description",
            ""
        )

        category = book.get(
            "category",
            "Unknown"
        )

        document_text = (
            f"Title: {title}\n"
            f"Author: {author}\n"
            f"Category: {category}\n"
            f"Description: {description}"
        )

        corpus.append(
            {
                "book_id": book.get("id"),
                "title": title,
                "author": author,
                "category": category,
                "description": description,
                "text": document_text
            }
        )

    return corpus


def save_corpus(
    corpus: list[dict]
):

    OUTPUT_FILE.parent.mkdir(
        parents=True,
        exist_ok=True
    )

    OUTPUT_FILE.write_text(
        json.dumps(
            corpus,
            indent=2,
            ensure_ascii=False
        ),
        encoding="utf-8"
    )


if __name__ == "__main__":

    try:

        books = fetch_books()

        print(
            f"Fetched {len(books)} books "
            "from the .NET API."
        )

        corpus = build_corpus(
            books
        )

        save_corpus(
            corpus
        )

        print(
            f"Created {len(corpus)} "
            "RAG documents."
        )

        print(
            f"Saved corpus to: {OUTPUT_FILE}"
        )

    except requests.RequestException as error:

        print(
            f"Failed to fetch books: {error}"
        )