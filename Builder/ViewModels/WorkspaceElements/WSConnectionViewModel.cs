using Shared.ViewModels;


namespace Builder.ViewModels.WorkspaceElements
{

    public class WSConnectionViewModel : ViewModelBase
    {
        private WorkspaceItemViewModel _source;
        public WorkspaceItemViewModel Source
        {

            get => _source;
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }
        private WorkspaceItemViewModel _target;
        public WorkspaceItemViewModel Target
        {
            get => _target;
            set
            {
                _target = value;
                OnPropertyChanged(nameof(Target));
            }
        }

        public WSConnectionViewModel(WorkspaceItemViewModel source, WorkspaceItemViewModel target)
        {
            Source = source;
            Target = target;
        }

        public WSConnectionViewModel()
        {
        }
    }
}
