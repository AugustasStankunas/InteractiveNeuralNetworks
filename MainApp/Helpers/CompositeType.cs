using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Builder.ViewModels;
using Builder.ViewModels.WorkspaceElements;
using MainApp.ViewModels;
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
