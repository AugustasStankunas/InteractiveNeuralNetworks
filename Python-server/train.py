import torch
from torch import nn
from torch.utils.data import DataLoader
from dataset import ClassificationDataset
from model import Model
import json
import os
import torchvision


# will have to make dynamic with config:
MAX_EPOCHS = 1000
CONFIG_PATH = "config.json"
DEVICE = "cuda" if torch.cuda.is_available() else "cpu"
VALID_INTERVAL = 10


def reset_log():
    if os.path.exists("log.txt"):
         os.remove("log.txt")


def log(text):
    with open("log.txt", "a") as f:
        f.write(text)


def train():
    reset_log()
    with open(CONFIG_PATH, "r") as f:
        config = json.load(f)

    BATCH_SIZE = config['Train']['BatchSize']
    LEARNING_RATE = config['Train']['LearningRate']
    TRAIN_DIR = os.path.join(config['Train']['DataDir'], "train")
    TEST_DIR = os.path.join(config['Train']['DataDir'], "test")
    if not os.path.exists(TEST_DIR): TEST_DIR = None
    VAL_DIR = os.path.join(config['Train']['DataDir'], "val")
    if not os.path.exists(VAL_DIR): VAL_DIR = None

    if config['Train']['LossFunction'] == 0:
        loss_fn = nn.MSELoss()
    elif config['Train']['LossFunction'] == 1:
        loss_fn = nn.L1Loss()
    elif config['Train']['LossFunction'] == 2:
        loss_fn = nn.BCELoss()
    elif config['Train']['LossFunction'] == 3:
        loss_fn = nn.CrossEntropyLoss()

    train_dataset = ClassificationDataset(TRAIN_DIR)
    test_dataset = ClassificationDataset(TEST_DIR) if TEST_DIR else None
    val_dataset = ClassificationDataset(VAL_DIR) if VAL_DIR else None
    train_dataloader = DataLoader(train_dataset, batch_size=BATCH_SIZE, shuffle=True)
    test_dataloader = DataLoader(test_dataset, batch_size=BATCH_SIZE, shuffle=True) if test_dataset else None
    val_dataloader = DataLoader(val_dataset, batch_size=BATCH_SIZE, shuffle=True) if val_dataset else None
    train_N = len(train_dataset)
    test_N = len(test_dataset)
    val_N = len(val_dataset)
    
    model = Model(config).to(DEVICE)
    optimizer = torch.optim.Adam(model.parameters(), lr=LEARNING_RATE)

    for epoch in range(1, MAX_EPOCHS):
        model.train()
        log(f"Epoch: {epoch}")
        total_loss_train = 0
        for X_train, y_train in train_dataloader:
            X_train = X_train.to(DEVICE)
            y_train = y_train.to(DEVICE)
            optimizer.zero_grad()
            output = model(X_train)
            model.reset_layers_outputs() # needed for my sphagetic logic to work
            loss = loss_fn(output, y_train)
            total_loss_train += loss.item()
            loss.backward()
            optimizer.step()
        log(f" | Train Loss: {total_loss_train}\n")

        if (epoch == 1 or epoch % VALID_INTERVAL == 0) and val_dataloader:
            log(f"Doing validation...\n")
            model.eval()
            total_loss_val = 0
            with torch.no_grad():
                for X_val, y_val in val_dataloader:
                    X_val = X_val.to(DEVICE)
                    y_val = y_val.to(DEVICE)
                    output = model(X_val)
                    model.reset_layers_outputs()
                    loss = loss_fn(output, y_val)
                    total_loss_val += loss
            log(f"Validation loss: {total_loss_val}\n")

        
    
            
