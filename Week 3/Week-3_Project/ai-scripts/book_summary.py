import os

from dotenv import load_dotenv
from groq import Groq


load_dotenv()

api_key = os.getenv("GROQ_API_KEY")

if not api_key:
    raise ValueError(
        "GROQ_API_KEY was not found."
    )


client = Groq(
    api_key=api_key
)


print("AI Book Summary Generator")
print("-" * 40)


title = input(
    "Enter book title: "
)

description = input(
    "Enter book description: "
)


prompt = f"""
Book title: {title}

Book description:
{description}

Write one concise paragraph summarizing this book
and suggest the most suitable genre.
"""


response = client.chat.completions.create(
    model="openai/gpt-oss-20b",
    messages=[
        {
            "role": "user",
            "content": prompt
        }
    ],
    max_tokens=250
)


print("\nAI Result:")

print(
    response
        .choices[0]
        .message
        .content
)