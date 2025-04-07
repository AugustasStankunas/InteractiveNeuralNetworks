import torch
from torch import nn
import json
from model import Model


CONFIG_PATH = "config.json"


def log(text):
    with open("log.txt", "w") as f:
        f.write(text)
        

def check_torch():
    output = f"{torch.__version__}\n{str(torch.cuda.is_available())}\n"
    log(output)


def build_dnn(config):
    pass

def main():
    with open(CONFIG_PATH, "r") as f:
        config = json.load(f)
    model = Model(config)


if __name__ == "__main__":
    main()