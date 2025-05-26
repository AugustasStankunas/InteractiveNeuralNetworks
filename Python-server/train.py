import torch
from torch import nn
from torch.utils.data import DataLoader
from dataset import ClassificationDataset
from model import Model, build_model
import json
import os
import torchvision
from torcheval.metrics import MulticlassAccuracy
from time import localtime, strftime


# will have to make dynamic with config:
MAX_EPOCHS = 200
CONFIG_PATH = "config.json"
DEVICE = "cuda" if torch.cuda.is_available() else "cpu"
VALID_INTERVAL = 10
SAVE_INTERVAL = 5


def reset_log():
    if os.path.exists("log.txt"):
         os.remove("log.txt")
    if os.path.exists("log1.txt"):
         os.remove("log1.txt")


def log(text):
    with open("log.txt", "a") as f:
        f.write(text)

def log1(text):
    with open("log1.txt", "a") as f:
        f.write(text)


def train():
    reset_log()
    with open(CONFIG_PATH, "r") as f:
        config = json.load(f)

    BATCH_SIZE = config['Train']['BatchSize']
    LEARNING_RATE = config['Train']['LearningRate']
    TRAIN_DIR = os.path.join(config['Train']['TrainDataPath'], "train")
    TEST_DIR = os.path.join(config['Train']['TrainDataPath'], "test")
    if not os.path.exists(TEST_DIR): TEST_DIR = None
    VAL_DIR = os.path.join(config['Train']['TrainDataPath'], "val")
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
    train_batches_N = len(train_dataset) / BATCH_SIZE
    test_batches_N = len(test_dataset) / BATCH_SIZE
    val_batches_N = len(val_dataset) / BATCH_SIZE
    
    model = build_model()
    optimizer = torch.optim.Adam(model.parameters(), lr=LEARNING_RATE)

    current_time = strftime('%Y-%m-%d-%H-%M-%S', localtime())
    os.mkdir(os.path.join("checkpoints", current_time))

    with open(os.path.join("checkpoints", current_time, "model_info.json"), "w") as f:
        json.dump({'labels': train_dataset.labels}, f)

    last_val_loss = 0
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
        total_loss_train /= train_batches_N
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
            total_loss_val /= val_batches_N
            last_val_loss = total_loss_val
            log(f"Validation loss: {total_loss_val}\n")
        
        if epoch % SAVE_INTERVAL == 0:
            model_name = f"checkpoint_{epoch}_{strftime('%Y-%m-%d-%H-%M-%S', localtime())}.pt"
            model_path = os.path.join("checkpoints", current_time, model_name)
            torch.save(model.state_dict(), model_path)
            log(f"Saving model as {model_path}\n")
            with open(CONFIG_PATH, "r") as f:
                config = json.load(f)
                with open(CONFIG_PATH, "w") as fw:
                    if "latest_checkpoint" in config.keys():
                        config['latest_checkpoint'] = model_path
                    else:
                        config.update({"latest_checkpoint": model_path})
                    json.dump(config, fw)
        log1(f"Epoch {epoch}: Train loss: {total_loss_train}    Validation loss: {last_val_loss}\n")

    log(f"Training finished.\n")
    log(f"Doing testing...\n")
    model.eval()
    total_loss_test = 0
    metric = MulticlassAccuracy()
    with torch.no_grad():
        for X_test, y_test in test_dataloader:
            X_test = X_test.to(DEVICE)
            y_test = y_test.to(DEVICE)
            output = model(X_test)
            model.reset_layers_outputs()
            loss = loss_fn(output, y_test)
            total_loss_test += loss
            metric.update(output, y_test)
    
    total_loss_test /= test_batches_N
    test_accuracy = metric.compute()
    log(f"Test loss: {total_loss_test}\n")
    log(f"Test accuracy: {test_accuracy}\n")
        
    
            
