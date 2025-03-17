using InteractiveNeuralNetworks.ViewModels.ToolbarElements;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class ToolbarViewModel : ViewModelBase
    {
        public BuilderViewModel Builder { get; set; }
        public WorkspaceItemViewModel WorkspaceItem { get; set; }
        public ObservableCollection<ToolbarItemViewModel> ToolbarItems { get; set; } = new();
		public ToolbarViewModel(BuilderViewModel builder) 
        {
            Builder = builder; 

            ToolbarItems.Add(new TBFullyConnectedViewModel(this));
            ToolbarItems.Add(new TBActivationFunctionViewModel(this));
            ToolbarItems.Add(new TBConvolutionViewModel(this));
            ToolbarItems.Add(new TBPoolingViewModel(this));
            ToolbarItems.Add(new TBConnectionViewModel(this));
        }
	}
}
