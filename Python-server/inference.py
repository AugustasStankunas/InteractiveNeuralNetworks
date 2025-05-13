import torch
from torch import nn
import os
from model import build_model
from torchvision.io import decode_image
import json
import sys


DEVICE = "cuda" if torch.cuda.is_available() else "cpu"
IMAGE_PATH_CONFIG = "predictImage.json"


def reset_log():
    if os.path.exists("log_inference.txt"):
        os.remove("log_inference.txt")


def log(text):
    with open("log_inference.txt", "a") as f:
        f.write(text)


def load_latest_checkpoint():
    checkpoint_parent_dirs = sorted(os.listdir("checkpoints"), key=lambda x: int(x.replace("-", "")))
    checkpoints = []
    for x in os.listdir(os.path.join("checkpoints", checkpoint_parent_dirs[-1])):
        if x[-1] == 't': 
            checkpoints.append(x)
    checkpoints = sorted(checkpoints, key=lambda x: int(x.replace("checkpoint_", "")[:x.replace("checkpoint_", "").find("_")]))
    parent_path = os.path.join('checkpoints', checkpoint_parent_dirs[-1])
    path = os.path.join(parent_path, checkpoints[-1])
    return torch.load(path), parent_path, path 


def inference(load_latest=False):
    reset_log()
    model = build_model()
    state_dict, checkpoint_dir_path, state_dict_path = load_latest_checkpoint()
    model.load_state_dict(state_dict)
    log(f"Loaded latest model from {state_dict_path}\n")
    with open(IMAGE_PATH_CONFIG, "r") as f:
        config = json.load(f)
    with open(os.path.join(checkpoint_dir_path, "model_info.json"), "r") as f:
        model_info = json.load(f)
    image = decode_image(config['PredictDataPath']).unsqueeze(0)
    image = image.to(DEVICE) / 255.0
    output = model(image)
    for i in model_info['labels'].keys():
        log(f"{model_info['labels'][i]}: {str(round(output[0][int(i)].item() * 100, 2))}%\n")


if __name__ == "__main__":
    inference()
