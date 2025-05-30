﻿using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using Builder.Enums;
using Shared.Attributes;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSOutputViewModel : WorkspaceItemViewModel
    {
        private int _inputNeurons;
        [EditableProperty]
        [Description(
            "Number of neurons in the incoming vector. " +
            "Must match the size of the previous layer’s output.\n" +
            "For example, if the prior layer outputs a 512‐dimensional feature, " +
            "set InputNeurons = 512."
        )]
        public int InputNeurons
        {
            get => _inputNeurons;
            set
            {
                _inputNeurons = value;
                OnPropertyChanged(nameof(InputNeurons));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private int _numOutputs;
        [EditableProperty("TextBox")]
        [Description(
             "Total number of output units produced by this layer.\n" +
             "Use one output per target dimension or class.\n" +
             "E.g. 10 for a 10-class classifier, or N for N-dimensional regression."
         )]
        public int NumOutputs
        {
            get => _numOutputs;
            set
            {
                _numOutputs = value;
                OnPropertyChanged(nameof(NumOutputs));
                OnPropertyChanged(nameof(DisplayName));
                //  GenerateOutputFields();
            }
        }

        private OutputType _outputParam;
        [EditableProperty("ComboBox")]
        [Description(
             "Output mode:\n" +
             "• Segmentation – produce pixel-wise class masks for each input.\n" +
             "• Classification – produce one class label or probability per sample."
         )]
        public OutputType OutputParam
        {
            get => _outputParam;
            set
            {
                _outputParam = value;
                OnPropertyChanged(nameof(ActivationFunction));
            }
        }
        /*  public ObservableCollection<string> OutputValues { get; set; } = new();

          private void GenerateOutputFields()
          {
              var oldValues = OutputValues.ToList();

              OutputValues.Clear();
              for (int i = 0; i < NumOutputs; i++)
              {
                  OutputValues.Add(i < oldValues.Count ? oldValues[i] : "");
              }
              OnPropertyChanged(nameof(OutputValues));
          }*/

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nN:{NumOutputs}";

        public WSOutputViewModel(int inputNeurons, int numOutputs, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            InputNeurons = inputNeurons;
            NumOutputs = numOutputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }

        [JsonConstructor]
        public WSOutputViewModel(int inputNeurons, int numOutputs, Point position, string name, ActivationFunctionType activationFunction)
            : base(position.X, position.Y, name: name, activationFunction: activationFunction)
        {
            InputNeurons = inputNeurons;
            NumOutputs = numOutputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}
