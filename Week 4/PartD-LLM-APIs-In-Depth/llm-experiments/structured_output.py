import json
import os

from dotenv import load_dotenv
from groq import Groq


load_dotenv()


api_key = os.getenv(
    "GROQ_API_KEY"
)

if not api_key:
    raise ValueError(
        "GROQ_API_KEY was not found."
    )


client = Groq(
    api_key=api_key
)


MODEL = "openai/gpt-oss-20b"


title = input(
    "Enter book title: "
)


description = input(
    "Enter book description: "
)


prompt = f"""
Return ONLY valid JSON.
Do not include markdown.
Do not include code fences.
Do not include any explanation.

Use exactly this structure:

{{
  "genre": "string",
  "summary": "one paragraph string"
}}

Book title:
{title}

Book description:
{description}
"""


response = client.chat.completions.create(
    model=MODEL,
    messages=[
        {
            "role": "user",
            "content": prompt
        }
    ],
    temperature=0,
    max_tokens=300
)


raw_output = (
    response
    .choices[0]
    .message
    .content
)


print(
    "\nRaw LLM Output:"
)

print(
    raw_output
)


try:

    parsed_data = json.loads(
        raw_output
    )


    print(
        "\nParsed Genre:"
    )

    print(
        parsed_data["genre"]
    )


except json.JSONDecodeError:

    print(
        "\nError: The LLM did not return valid JSON."
    )


except KeyError:

    print(
        "\nError: JSON did not contain the expected genre field."
    )