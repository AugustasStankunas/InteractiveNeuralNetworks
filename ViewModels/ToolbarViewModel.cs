using InteractiveNeuralNetworks.Commands;
using InteractiveNeuralNetworks.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace InteractiveNeuralNetworks.ViewModels
{
    public class ToolbarViewModel : ViewModelBase
    {
        public ICommand MouseMoveCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

        public BuilderViewModel Builder { get; set; }

        public ObservableCollection<ToolbarItemViewModel> ToolbarItems { get; set; } = new();
        public ToolbarViewModel(BuilderViewModel builder) 
        {
            //MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            //DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            //MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            //MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);

            Builder = builder; 

            ToolbarItems.Add(new ToolbarItemViewModel(this, "OrangeControl", "Orange", "name1"));
            ToolbarItems.Add(new ToolbarItemViewModel(this, "PinkControl", "Pink", "name2"));
            ToolbarItems.Add(new ToolbarItemViewModel(this, "BlueControl", "LightBlue", "name3"));
        }

        //private void OnDragOver(DragEventArgs e)
        //{
        //    //if (e.Data.GetDataPresent(typeof(WorkspaceItemViewModel)))
        //    //{
        //    //    var draggedItem = e.Data.GetData(typeof(WorkspaceItemViewModel)) as WorkspaceItemViewModel;
        //    //    Point dropPosition = e.GetPosition(e.Source as IInputElement);
        //    //    if (draggedItem != null)
        //    //    {
        //    //        dropPosition.X -= mouseOffset.X;
        //    //        dropPosition.Y -= mouseOffset.Y;
        //    //        draggedItem.Position = dropPosition;
        //    //    }

        //    //}

        //}

        //private void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    Point mousePos = e.GetPosition(null);
        //    WorkspaceItemViewModel selectedItem = WorkspaceItemCreator.GetWorkspaceItem("OrangeControl", mousePos);
        //    selectedItem.Opacity = 0.5;
        //    Builder.WorkspaceItemSelected.Add(selectedItem);

        //    var data = e.OriginalSource as FrameworkElement;
        //    DragDrop.DoDragDrop(e.OriginalSource as UIElement, new DataObject(typeof(WorkspaceItemViewModel), data.DataContext), DragDropEffects.Move);
        //}

        //private void OnMouseMove(MouseEventArgs e)
        //{

        //}

        //private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        //{
        //    Builder.WorkspaceItemSelected.Clear();
        //}

    }
}
