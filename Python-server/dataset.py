import torch
from torch import nn
from torch.utils.data import Dataset
import os
from torchvision.io import decode_image


DEVICE = "cuda" if torch.cuda.is_available() else "cpu"


class ClassificationDataset(Dataset):
    def __init__(self, dir):
        self.data = []
        self.labels = {}
        for i, label in enumerate(os.listdir(dir)):
            self.labels.update({i: label})
            for image_name in os.listdir(os.path.join(dir, label)):
                self.data.append((os.path.join(dir, label, image_name), i))


    def __len__(self):
        return len(self.data)


    def __getitem__(self, index):
        img_path, label = self.data[index]
        image = decode_image(img_path)
        image = image.to(DEVICE) / 255.0
        return image, label