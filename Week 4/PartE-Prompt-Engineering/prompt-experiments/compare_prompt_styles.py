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


title = "The Martian"

description = (
    "An astronaut becomes stranded on Mars "
    "and must use science and engineering "
    "to survive."
)


def call_model(
    messages,
    temperature=0
):

    response = client.chat.completions.create(
        model=MODEL,
        messages=messages,
        temperature=temperature,
        max_tokens=250
    )

    return (
        response
        .choices[0]
        .message
        .content
    )


zero_shot_prompt = f"""
Suggest a genre and a short summary
for this book.

Title:
{title}

Description:
{description}
"""


few_shot_prompt = f"""
Classify the genre and write a short summary.

Examples:

Book:
Dune

Description:
A desert planet, political intrigue,
and a struggle over a powerful resource.

Genre:
Science Fiction

Summary:
A young noble becomes involved in political
conflict and survival on a dangerous desert planet.


Book:
Pride and Prejudice

Description:
Manners, marriage, and relationships
in 19th century England.

Genre:
Romance

Summary:
A young woman navigates family expectations,
social class, and an evolving relationship.


Now classify this book.

Book:
{title}

Description:
{description}

Genre:
"""


system_messages = [
    {
        "role": "system",
        "content":
            "You are a library cataloguer. "
            "Return exactly two lines. "
            "Line 1 must begin with 'Genre:'. "
            "Line 2 must begin with 'Summary:'. "
            "Do not add any other text."
    },
    {
        "role": "user",
        "content": f"""
Book title:
{title}

Book description:
{description}
"""
    }
]


print(
    "\nZERO-SHOT OUTPUT"
)

print(
    "-" * 50
)

print(
    call_model(
        [
            {
                "role": "user",
                "content": zero_shot_prompt
            }
        ]
    )
)


print(
    "\nFEW-SHOT OUTPUT"
)

print(
    "-" * 50
)

print(
    call_model(
        [
            {
                "role": "user",
                "content": few_shot_prompt
            }
        ]
    )
)


print(
    "\nSYSTEM ROLE OUTPUT"
)

print(
    "-" * 50
)

print(
    call_model(
        system_messages
    )
)