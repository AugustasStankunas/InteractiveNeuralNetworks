import torch
from torch import nn
import modules

class Model(nn.Module):
    def __init__(self, config):
        super().__init__()
        self.config = config
        self.connections = []
        model_layers = torch.nn.ModuleDict()
        model_layers_info = {}
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
                current_layer = modules.Add() # I hope it will work
            elif layer_config['$type'] == "BatchNorm":
                current_layer = nn.BatchNorm2d()
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

            # Inputs - list of layer names that are inputs for this layer
            # Output - this layer forward result
            model_layers.update(torch.nn.ModuleDict({layer_config['Name']: current_layer}))
            model_layers_info.update({layer_config['Name']:  {'Inputs': [], 
                                                              'Output': None, 
                                                              'ForwardPassPoss': layer_config['Layer']}})

        self.model_layers = model_layers
        self.model_layers_info = model_layers_info

        # self.connections.append(self.config['Connections'][0]['Source']['Name']) # Will have to change it when we add input layer
        for connection in self.config['Connections']:
            source_name = connection['Source']['Name']
            target_name = connection['Target']['Name']
            for layer_name in self.model_layers_info.keys():
                if layer_name == target_name:
                    self.model_layers_info[layer_name]['Inputs'].append(source_name)
        
        #print(self.model_layers_info)
                
    
    def forward(self, X):
        passed_all_layers = False
        # Iterating through layers until input passed through all layers
        while not passed_all_layers:
            passed_all_layers = True
            # Iterating through layers to find ones that have available input
            for layer_name in self.model_layers.keys(): 
                layer_info = self.model_layers_info[layer_name]
                layer = self.model_layers[layer_name]
                # Found input layer
                if layer_info['ForwardPassPoss'] == 0 and layer_info['Output'] is None:
                    #print(f"Passed input layer {layer_name}")
                    passed_all_layers = True
                    layer_info['Output'] = layer(X) # Input layer forward pass
                    #print(f"Out shape: {layer_info['Output'].shape}")
                    continue
                # Check if all input layers have been already forwarded
                if sum([1 if self.model_layers_info[input_name]['Output'] is None else 0 for input_name in layer_info['Inputs']]) == 0:
                    #print(f"Passed hidden layer {layer_name}")
                    passed_all_layers = True
                    # Hidden layer forward pass
                    layer_info['Output'] = layer(*[self.model_layers_info[input_name]['Output'] for input_name in layer_info['Inputs']])
                    #print(f"Out shape: {layer_info['Output'].shape}")
        # Finding output layer and returning output
        for layer_name in self.model_layers_info.keys():
            layer_info = self.model_layers_info[layer_name]
            if layer_info['ForwardPassPoss'] == 1:
                #print(f"Output layer: {layer_name}")
                #print(layer_info['Output'].shape)
                return layer_info['Output']


    def reset_layers_outputs(self):
        for layer_name in self.model_layers_info.keys():
            self.model_layers_info[layer_name]['Output'] = None


