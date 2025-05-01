using System.Windows;
using System.Windows.Input;
using Builder.Enums;
using Builder.Helpers;
using Shared.Commands;
using Shared.ViewModels;


namespace Builder.ViewModels
{
    public class ToolbarItemViewModel : ViewModelBase
    {
        public ToolbarViewModel Toolbar { get; set; }

        public WorkspaceItemViewModel WorkspaceItem { get; set; }

        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand MouseEnterCommand { get; }
        public ICommand MouseLeaveCommand { get; }

        private string _name = default!;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));

                if (value)
                {
                    Mouse.AddMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
                }
            }
        }

        public void GlobalMouseUpHandler(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                IsSelected = false;
                Toolbar.Builder.WorkspaceItemSelected.Clear();
                Mouse.RemoveMouseUpHandler(Application.Current.MainWindow, GlobalMouseUpHandler);
            }
        }


        public bool _isHovered;
        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                _isHovered = value;
                OnPropertyChanged(nameof(IsHovered));
            }
        }

        public string TooltipText { get; set; } = LayerType.Default.GetDescription();

        public ToolbarItemViewModel(ToolbarViewModel toolbar)
        {
            Toolbar = toolbar;
            WorkspaceItem = new WorkspaceItemViewModel();
            Name = "ToolbarItem";

            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
            MouseEnterCommand = new RelayCommand<MouseEventArgs>(OnMouseEnter);
            MouseLeaveCommand = new RelayCommand<MouseEventArgs>(OnMouseLeave);
        }

        public ToolbarItemViewModel(ToolbarViewModel toolbar, string controlType, string color, string name)
        {
            Toolbar = toolbar;

            Name = name;
            WorkspaceItem = new WorkspaceItemViewModel();

            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
            MouseEnterCommand = new RelayCommand<MouseEventArgs>(OnMouseEnter);
            MouseLeaveCommand = new RelayCommand<MouseEventArgs>(OnMouseLeave);
        }

        // Overridina ToolbarElements 
        public virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //IsSelected = true;  //ToolbarElement ant kiekvieno atskirai reikia ideti atm.
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel selectedItem = new WorkspaceItemViewModel(mousePos.X, mousePos.Y, 60, 60);
            selectedItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(selectedItem);
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Toolbar.Builder.WorkspaceItemSelected.Clear();
            IsSelected = false;
        }

        private void OnMouseEnter(MouseEventArgs e)
        {
            IsHovered = true;
        }

        private void OnMouseLeave(MouseEventArgs e)
        {
            IsHovered = false;
        }

    }
}
