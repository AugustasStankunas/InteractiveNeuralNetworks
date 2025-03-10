using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ToolbarItemViewModel(ToolbarViewModel toolbar) 
        { 
            Toolbar = toolbar;
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
        }

        public ToolbarItemViewModel(ToolbarViewModel toolbar, string controlType, string color, string name) 
        {
            Toolbar = toolbar;
            ControlType = controlType;

            Color = color;
            Name = name;

            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
        }

        public virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel selectedItem = new WorkspaceItemViewModel(mousePos.X, mousePos.Y, 60, 60, "Neon");
            selectedItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(selectedItem);
        }

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Toolbar.Builder.WorkspaceItemSelected.Clear();
        }
    }
}
