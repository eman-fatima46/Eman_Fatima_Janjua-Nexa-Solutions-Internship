from rag_pipeline import (
    build_vector_store,
    retrieve
)


chunk_count = build_vector_store()


print(
    f"Stored {chunk_count} chunks in Chroma."
)


question = (
    "Which books are science fiction?"
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


print(
    "\n--- Metadata ---"
)


for metadata in metadatas:

    print(
        metadata
    )