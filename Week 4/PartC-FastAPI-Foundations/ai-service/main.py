from fastapi import FastAPI
from pydantic import BaseModel


app = FastAPI(
    title="Library AI Service",
    description="Week 4 FastAPI foundations for the Library AI track.",
    version="1.0.0"
)


class SummaryRequest(BaseModel):
    title: str
    description: str


class GenreSuggestionRequest(BaseModel):
    title: str
    description: str


@app.get("/health")
def health():
    return {
        "status": "ok"
    }


@app.post("/summarize")
def summarize(
    request: SummaryRequest
):
    return {
        "title": request.title,
        "received": True
    }


@app.post("/genre-suggestion")
def genre_suggestion(
    request: GenreSuggestionRequest
):
    return {
        "title": request.title,
        "genre": "Placeholder Genre",
        "received": True
    }