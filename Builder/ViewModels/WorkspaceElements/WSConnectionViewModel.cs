using Shared.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


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

		private bool _IsSelected; 

		public bool IsSelected
		{
			get => _IsSelected;
			set
			{
				_IsSelected = value;
				OnPropertyChanged(nameof(IsSelected));

				if (value)
					Mouse.AddMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
				else
					Mouse.RemoveMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
				
			}
		}
		public void GlobalMouseUpHandler(object sender, MouseButtonEventArgs e)
		{
			
			if (e.ChangedButton == MouseButton.Left)
			{
				var element = e.OriginalSource as FrameworkElement;
				if (element.DataContext != this)
				{
					IsSelected = false;
				}
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
