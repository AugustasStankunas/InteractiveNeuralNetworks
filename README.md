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
