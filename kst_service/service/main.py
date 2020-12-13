import uvicorn
import pandas as pd
import json
from fastapi import FastAPI
from pydantic import BaseModel
import sys
sys.path.append('learning_spaces/')
from learning_spaces.kst import iita
import random

async def calculate(dict):
    data_frame = pd.DataFrame(dict)
    response = iita(data_frame, v=1)
    return response


app = FastAPI()


@app.post("/kst/iita", response_model=str)
async def create_real_knowledge_space(problems: list):

    dictionary = {}
    with open('pisa.txt', 'r') as f:
        questions = f.readline().split()

        for problem in problems:
            dictionary[problem] = []

        content = f.readlines()
        content = [x.strip() for x in content]
        for line in content:
            split = list(filter(None, line.split(' ')))
            for (i, problem) in enumerate(problems):
                dictionary[problem].append(int(split[i+1]) if len(split) > i+1 else random.randint(0, 1))

    result = await calculate(dictionary)
    return json.dumps(result["implications"])


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
