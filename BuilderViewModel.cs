using InteractiveNeuralNetworks.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveNeuralNetworks
{
    internal class BuilderViewModel : ViewModelBase
    {
        public BuilderModel Model { get; set; }

        public RelayCommand ClickMeButtonCommand { get; set; }

        public int Counter
        {
            get => Model.ClickCounter;
            set
            {
                Model.ClickCounter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }

        public BuilderViewModel()
        {
            Model = new BuilderModel();

            ClickMeButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
        }

        // Button methods
        private void ExecuteClickMe(object obj)
        {
            Counter++;
        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }
    }
}
