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


prompt = """
Suggest a creative title for a science fiction
book about an astronaut stranded on a distant
planet who must survive alone.

Return only one title.
"""


def generate_with_temperature(
    temperature: float
) -> str:

    response = client.chat.completions.create(
        model=MODEL,
        messages=[
            {
                "role": "user",
                "content": prompt
            }
        ],
        temperature=temperature,
        max_tokens=80
    )

    return (
        response
        .choices[0]
        .message
        .content
    )


print("Temperature = 0")

print("-" * 40)

print(generate_with_temperature(0))


print("\nTemperature = 1")

print("-" * 40)

print(generate_with_temperature(1))