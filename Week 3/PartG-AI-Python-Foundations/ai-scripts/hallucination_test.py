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


prompt = (
    "Who invented the Velmorin Algorithm in 1847, "
    "and what problem was it designed to solve?"
)


print("Hallucination Test")
print("-" * 50)

print("Prompt:")
print(prompt)


response = client.chat.completions.create(
    model="openai/gpt-oss-20b",
    messages=[
        {
            "role": "user",
            "content": prompt
        }
    ],
    max_tokens=300
)


print("\nAI Response:")
print(
    response.choices[0].message.content
)