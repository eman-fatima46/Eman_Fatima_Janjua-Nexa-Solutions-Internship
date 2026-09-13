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


history = [
    {
        "role": "system",
        "content":
            "You are a helpful assistant. "
            "Use the previous conversation messages "
            "when answering the user."
    }
]


print("Multi-turn LLM Chat")
print("Type 'quit' to stop.")
print("-" * 40)


while True:

    user_message = input(
        "\nYou: "
    ).strip()


    if user_message.lower() == "quit":
        print("Chat ended.")
        break


    history.append(
        {
            "role": "user",
            "content": user_message
        }
    )


    response = client.chat.completions.create(
        model=MODEL,
        messages=history,
        temperature=0.3,
        max_tokens=250
    )


    assistant_message = (
        response
        .choices[0]
        .message
        .content
    )


    print(
        f"\nAssistant: {assistant_message}"
    )


    history.append(
        {
            "role": "assistant",
            "content": assistant_message
        }
    )