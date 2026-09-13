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


MODEL = "openai/gpt-oss-20b"


print("Streaming Example")
print("-" * 40)


stream = client.chat.completions.create(
    model=MODEL,
    messages=[
        {
            "role": "user",
            "content":
                "Explain LLM streaming "
                "in two simple sentences."
        }
    ],
    temperature=0.3,
    max_tokens=150,
    stream=True
)


print("\nResponse:\n")


for chunk in stream:

    content = (
        chunk
        .choices[0]
        .delta
        .content
    )

    if content:

        print(
            content,
            end="",
            flush=True
        )


print()