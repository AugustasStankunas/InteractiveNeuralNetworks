﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Builder.ViewModels.WorkspaceElements
{
     class WSBatchNormViewModel: WorkspaceItemViewModel
    {
        private double _momentum;
        [Attributes.EditableProperty]
        public double Momentum
        {
            get => _momentum;
            set
            {
                _momentum = value;
                OnPropertyChanged(nameof(Momentum));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        private double _epsilon;
        [Attributes.EditableProperty]
        public double Epsilon
        {
            get => _epsilon;
            set
            {
                _epsilon = value;
                OnPropertyChanged(nameof(Epsilon));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
      
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nM:{Momentum} E:{Epsilon}";

        public WSBatchNormViewModel(double momentum, double epsilon, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            Momentum = momentum;
            Epsilon = epsilon;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "BatchNorm.png");
        }
    }
}
