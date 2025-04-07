import torch
from torch import nn


class Model(nn.Module):
    def __init__(self, config):
        super(Model).__init__()
        model_layers = {}
        for layer_config in config['Items']:
            if layer_config['$type'] == "Linear":
                current_layer = nn.Linear(in_features=layer_config['InputNeurons'],
                                          out_features=layer_config['OutputNeurons'])
            elif layer_config['$type'] == "Conv2D":
                current_layer = nn.Conv2d(in_channels=layer_config['InputChannels'], 
                                          out_channels=layer_config['OutputChannels'],
                                          kernel_size=layer_config['KernelSize'],
                                          stride=layer_config['Stride'],
                                          padding="same") # made padding same by default. Maybe we could make it customizable too?
            elif layer_config['$type'] == "Pool2D":
                if layer_config['PoolingType'] == 0: # Average
                    current_layer = nn.AvgPool2d(kernel_size=layer_config['KernelSize'],
                                                 stride=layer_config['Stride'])
                elif layer_config['PoolingType'] == 1: # Max
                    current_layer = nn.MaxPool2d(kernel_size=layer_config['KernelSize'],
                                                 stride=layer_config['Stride'])
            elif layer_config['$type'] == "Add":
                pass # need custom implementation
            elif layer_config['$type'] == "BatchNorm":
                pass # need a little bit more thoughtful implementation
            elif layer_config['$type'] == "Flatten":
                current_layer = nn.Flatten()
            elif layer_config['$type'] == "Dropout":
                current_layer = nn.Dropout(layer_config['Rate'])
            
            if layer_config['ActivationFunction'] == 1:
                current_layer = nn.Sequential(current_layer, nn.Sigmoid())
            elif layer_config['ActivationFunction'] == 2:
                current_layer = nn.Sequential(current_layer, nn.ReLU())
            elif layer_config['ActivationFunction'] == 3:
                current_layer = nn.Sequential(current_layer, nn.Tanh())
            elif layer_config['ActivationFunction'] == 4: 
                pass # Linear activation
            elif layer_config['ActivationFunction'] == 5:
                current_layer = nn.Sequential(current_layer, nn.Softmax())

            model_layers.update({layer_config['Name']: current_layer})
        print(model_layers)
                
    
    def forward():
        pass
