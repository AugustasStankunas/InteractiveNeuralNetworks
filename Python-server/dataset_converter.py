import os
from torchvision.datasets import FashionMNIST, CIFAR10
from torchvision.transforms import ToPILImage
from pathlib import Path


"""
Method to save fashion MNIST as raw images.
Maybe we should let user choose to use any existing Pytorch dataset,
that wouldn't really be difficult, but I think it's better to give user more control by letting them use custom images.
"""
def save_fashion_mnist(torch_dataset, root_dir="FashionMnistImg", image_format="png"):
    train_dir = os.path.join(root_dir, "train")
    test_dir = os.path.join(root_dir, "test")
    val_dir = os.path.join(root_dir, "val")
    
    train_dataset = torch_dataset(root=".", train=True, download=True)
    test_dataset  = torch_dataset(root=".", train=False, download=True)
    
    class_names = train_dataset.classes 
    test_dataset_size = len(test_dataset) // 10 # divided by class count
    
    for split_dir in (train_dir, test_dir, val_dir):
        for cls in class_names:
            os.makedirs(os.path.join(split_dir, cls.replace('/', ',')), exist_ok=True)
    
    def save_split(ds, save_val=False):
        for idx in range(len(ds)):
            img, label = ds[idx]
            cls_name = class_names[label]
            filename = f"{idx:05d}.{image_format}"
            if not save_val:
                path = os.path.join(train_dir, cls_name.replace('/', ','), filename)
            else:
                if len(os.listdir(os.path.join(test_dir, cls_name.replace('/', ',')))) >= test_dataset_size // 2:
                    path = os.path.join(val_dir, cls_name.replace('/', ','), filename)
                else:
                    path = os.path.join(test_dir, cls_name.replace('/', ','), filename)
            img.save(path)
            if idx % 5000 == 0:
                print(f"Saved {idx} images")
    
    # Pour out the images
    print("Saving training images")
    save_split(train_dataset, False)
    print("Saving test / val images")
    save_split(test_dataset, True)
    print("Finished.")

if __name__ == "__main__":
    save_fashion_mnist(CIFAR10, "Cifar10DatasetImg")