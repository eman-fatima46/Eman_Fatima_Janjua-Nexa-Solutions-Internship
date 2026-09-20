import chromadb


chroma_client = chromadb.Client()


collection = chroma_client.create_collection(
    name="books_filter_demo"
)


collection.add(
    documents=[
        "A young wizard attends a magic school and fights a dark lord.",
        "A crew travels through a wormhole to save humanity from a dying Earth.",
        "Two feuding families in 19th century England navigate love and marriage.",
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
    ],
    ids=[
        "book_1",
        "book_2",
        "book_3"
    ],
)


results = collection.query(
    query_texts=[
        "magic adventure"
    ],
    n_results=1,
    where={
        "category": "Fantasy"
    }
)


print(
    "\n--- Filtered Search Results ---"
)

print(
    "Documents:"
)

print(
    results["documents"]
)

print(
    "\nMetadata:"
)

print(
    results["metadatas"]
)