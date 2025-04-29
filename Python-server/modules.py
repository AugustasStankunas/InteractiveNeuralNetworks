import torch
from torch import nn


class Add(nn.Module):
    def __init__(self):
        super().__init__()

    
    def forward(self, X1, X2):
        return torch.add(X1, X2)