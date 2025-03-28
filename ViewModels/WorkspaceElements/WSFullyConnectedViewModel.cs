﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InteractiveNeuralNetworks.ViewModels.WorkspaceElements
{
    class WSFullyConnectedViewModel : WorkspaceItemViewModel
    {
        private int _inputNeurons;
        [Attributes.EditableProperty]
        public int InputNeurons
        {
            get => _inputNeurons;
            set
            {
                _inputNeurons = value;
                OnPropertyChanged(nameof(InputNeurons));
            }
        }

        private int _outputNeurons;
        [Attributes.EditableProperty]
        public int OutputNeurons
        {
            get => _outputNeurons;
            set
            {
                _outputNeurons = value;
                OnPropertyChanged(nameof(OutputNeurons));
            }
        }

        public WSFullyConnectedViewModel(int inputNeurons, int outputNeurons, double x, double y, int width, int height, double opacity = 1)
            : base(x, y, width, height, opacity)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "fullyConnected.png"); ;
        }
    }
}
