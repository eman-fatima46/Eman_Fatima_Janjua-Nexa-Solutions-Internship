import chromadb


chroma_client = chromadb.Client()


collection = chroma_client.create_collection(
    name="expanded_books_demo"
)


collection.add(
    documents=[
        "A young wizard attends a magic school and fights a dark lord.",

        "A crew travels through a wormhole to save humanity from a dying Earth.",

        "Two feuding families in 19th century England navigate love and marriage.",

        "A detective investigates a mysterious murder in a quiet English village.",

        "A group of explorers discover ancient ruins hidden deep inside a jungle.",
    ],
    metadatas=[
        {
            "source": "book_1.txt",
            "category": "Fantasy"
        },
        {
            "source": "book_2.txt",
            "category": "Science Fiction"
        },
        {
            "source": "book_3.txt",
            "category": "Romance"
        },
        {
            "source": "book_4.txt",
            "category": "Mystery"
        },
        {
            "source": "book_5.txt",
            "category": "Adventure"
        },
    ],
    ids=[
        "book_1",
        "book_2",
        "book_3",
        "book_4",
        "book_5"
    ],
)


queries = [
    "a murder investigation",
    "an adventure exploring ancient places",
    "a story about travelling through space"
]


for query in queries:

    print(
        "\n================================"
    )

    print(
        "Query:",
        query
    )


    results = collection.query(
        query_texts=[
            query
        ],
        n_results=1
    )


    print(
        "Best match:"
    )

    print(
        results["documents"][0][0]
    )


    print(
        "Metadata:"
    )

    print(
        results["metadatas"][0][0]
    )