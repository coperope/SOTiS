import uvicorn
import pandas as pd
import json
from fastapi import FastAPI
from pydantic import BaseModel
import sys
from typing import List
sys.path.append('learning_spaces/')
from learning_spaces.kst import iita


async def calculate(dict):
    data_frame = pd.DataFrame(dict)
    response = iita(data_frame, v=1)
    return response


app = FastAPI()


@app.post("/kst/iita", response_model=str)
async def create_real_knowledge_space(matrix: dict):
    result = await calculate(matrix)
    return json.dumps(result["implications"])


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
