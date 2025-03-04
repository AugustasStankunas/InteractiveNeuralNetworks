using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class BuilderViewModel : ViewModelBase
    {
        public BuilderModel Model { get; set; }
        public WorkspaceViewModel WorkspaceViewModel { get; set; }

        // Hello world
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
            WorkspaceViewModel = new WorkspaceViewModel();

            ClickMeButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
        }

        // Button methods
        private void ExecuteClickMe(object obj)
        {
            Counter++;
            //CanvasViewModel.CanvasItems.Add(new CanvasItemViewModel(50, 50, "Black"));

        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }
    }
}
