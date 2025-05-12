# Interactive neural networks
This is a repository of a project for a KTU module "Programming Engineering", made by four 2nd year students - Juozapas Jurkša, Ugnius Varžukas, Ignas Nakčeris and Augustas Stankūnas from group IFD-3/2. 

Using this app the user can build, train and test neural networks for image classification and segmentation.

## Features

### Neural network layer types and their editable properties:
- Input (image dimensions, number of channels)
- Output (number of output classes, segmentation|classification selection)
- Fully Connected (input and output size)
- Convolutional (input and output channgels, kernel size, stride)
- Pooling (Max or Average, kernel size, stride)
- Batch Normalization (input channels, momentum, epsilon)
- Flatten
- Dropout (rate)
- Add (number of inputs)

### Activation function types:
- Sigmoid
- ReLU
- Tanh
- Linear
- SoftMax

### Hyperparameters:
- Learning rate
- Loss function:
  - MSE
  - MAE
  - Binary Cross Entropy
  - Categorical Cross Entropy
- Batch size
- Augmentations:
  - Horizontal flip
  - Vertical flip
  - Pad
  - Zoom out
  - Rotation
  - Affine
  - Perspective

## How to use

The program has 3 windows - Builder, Train and Test. The model can be saved to a .JSON file by the File menu item.

### Builder

This window is for building the actual model. The toolbar in the left contains all neural network layers, which can be dragged and dropped on the main canvas. When a layer is selected, its properties are shown in the properties window on the right. Connection mode can either be turned on or off. When connection mode is ON, layers cannot be moved, only connections can be created. Layers and connections can be deleted by pressing the Delete button when selected. Multi selection is also implemented with Ctrl + LMB.

### Train

### Test

## How to launch

## Testing and its results
