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


prompts = [
    {
        "name": "Plain Question",
        "prompt": "What is a token in an LLM?"
    },
    {
        "name": "Direct Instruction",
        "prompt": (
            "Explain what a token is in an LLM "
            "using exactly two simple sentences."
        )
    },
    {
        "name": "Role-Based Prompt",
        "prompt": (
            "You are a strict librarian who only "
            "answers in one sentence. Explain what "
            "a token is in an LLM."
        )
    }
]


for item in prompts:

    print("\n" + "=" * 50)
    print(item["name"])
    print("=" * 50)

    print("Prompt:")
    print(item["prompt"])

    response = client.chat.completions.create(
        model="openai/gpt-oss-20b",
        messages=[
            {
                "role": "user",
                "content": item["prompt"]
            }
        ],
        max_tokens=300
    )

    print("\nResponse:")
    print(
        response.choices[0].message.content
    )