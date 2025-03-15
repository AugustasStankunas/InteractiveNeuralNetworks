using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class ToolbarItemViewModel : ViewModelBase
    {
        public string ControlType { get; set; }
        public ToolbarViewModel Toolbar { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
			
        public ICommand MouseMoveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
		public ICommand MouseEnterCommand { get; }
		public ICommand MouseLeaveCommand { get; }

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

		public ToolbarItemViewModel(ToolbarViewModel toolbar) 
        {
            Toolbar = toolbar;
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
			MouseEnterCommand = new RelayCommand<MouseEventArgs>(OnMouseEnter);
			MouseLeaveCommand = new RelayCommand<MouseEventArgs>(OnMouseLeave);
		}

        public ToolbarItemViewModel(ToolbarViewModel toolbar, string controlType, string color, string name) 
        {
            Toolbar = toolbar;
            ControlType = controlType;

            Color = color;
            Name = name;

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
			WorkspaceItemViewModel selectedItem = new WorkspaceItemViewModel(mousePos.X, mousePos.Y, 60, 60, "Black");
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
