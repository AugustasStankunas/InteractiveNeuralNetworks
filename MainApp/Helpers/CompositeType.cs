using System.Collections.ObjectModel;
using Builder.ViewModels;
using Builder.ViewModels.WorkspaceElements;
using Train.ViewModels;

namespace Train.Helpers
{
    internal class CompositeType
    {
        public ObservableCollection<WorkspaceItemViewModel> Items { get; set; } = new ObservableCollection<WorkspaceItemViewModel>();
        public ObservableCollection<WSConnectionViewModel> Connections { get; set; } = new ObservableCollection<WSConnectionViewModel>();
        public TrainViewModel Train { get; set; } = new TrainViewModel();
    }
}
