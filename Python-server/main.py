import torch
from torch import nn
import json
from model import Model
import train


def log(text):
    with open("log.txt", "w") as f:
        f.write(text)
    

def check_torch():
    output = f"{torch.__version__}\n{str(torch.cuda.is_available())}\n"
    log(output)


def main():
    # with open(CONFIG_PATH, "r") as f:
    #     config = json.load(f)
    # dummy_input = torch.zeros((1, 1, 64, 64)).to(DEVICE)
    # model = Model(config).to(DEVICE)
    # output = model(dummy_input)
    # print(output)
    train.train()


if __name__ == "__main__":
    main()