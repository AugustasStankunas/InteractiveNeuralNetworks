using System.Collections.ObjectModel;
using Builder.ViewModels.ToolbarElements;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class ToolbarViewModel : ViewModelBase
    {
        public BuilderViewModel Builder { get; set; }
        public ObservableCollection<ToolbarItemViewModel> ToolbarItems { get; set; } = new();
        public ToolbarViewModel(BuilderViewModel builder)
        {
            Builder = builder;

            ToolbarItems.Add(new TBFullyConnectedViewModel(this));
            ToolbarItems.Add(new TBConvolutionViewModel(this));
            ToolbarItems.Add(new TBPoolingViewModel(this));
            ToolbarItems.Add(new TBBatchNormViewModel(this));
            ToolbarItems.Add(new TBFlattenViewModel(this));
            ToolbarItems.Add(new TBDropoutViewModel(this));
            ToolbarItems.Add(new TBAddViewModel(this));
            ToolbarItems.Add(new TBConnectionViewModel(this));
        }
    }
}
