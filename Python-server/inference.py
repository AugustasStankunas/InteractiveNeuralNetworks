import torch
from torch import nn
import os


def load_latest_checkpoint_state_dict():
    checkpoint_parent_dirs = sorted(os.listdir("checkpoints"), key=lambda x: int(x.replace("-", "")))
    checkpoints = os.listdir(os.path.join("checkpoints", checkpoint_parent_dirs[-1]))
    checkpoints = sorted(checkpoints, key=lambda x: int(x.replace("checkpoint_", "")[:x.replace("checkpoint_", "").find("_")]))
    return torch.load(os.path.join("checkpoints", checkpoint_parent_dirs[-1], checkpoints[-1]))


# def inference()

# if __name__ == "__main__":
#     load_latest_checkpoint()
