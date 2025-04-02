using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ViewModels;
using Shared.Attributes;

using Builder.Enums;

namespace Train.ViewModels
{
    public class TrainViewModel : ViewModelBase
    {
        private double _learningRate;
        [EditableProperty]
        public double LearningRate
        {
            get => _learningRate;
            set
            {
                _learningRate = value;
                OnPropertyChanged(nameof(LearningRate));
            }
        }

        private LossFunctionType _lossFunction;
        [EditableProperty("ComboBox")]
        public LossFunctionType LossFunction
        {
            get => _lossFunction;
            set
            {
                _lossFunction = value;
                OnPropertyChanged(nameof(LossFunction));
            }
        }

        private int _batchSize;
        [EditableProperty]
        public int BatchSize
        {
            get => _batchSize;
            set
            {
                _batchSize = value;
                OnPropertyChanged(nameof(BatchSize));
            }
        }

        public HyperparametersWindowViewModel HyperparametersWindowViewModel { get; set; }

        public TrainViewModel()
        {
            InitializeHyperparameters();
            HyperparametersWindowViewModel = new HyperparametersWindowViewModel(this);
        }

        private void InitializeHyperparameters()
        {
            LearningRate = 0.01;
            LossFunction = LossFunctionType.MSE;
            BatchSize = 32;
        }
    }
}
