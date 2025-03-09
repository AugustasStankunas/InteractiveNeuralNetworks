﻿using InteractiveNeuralNetworks.Commands;
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
    public class ToolbarItemViewModel
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
        }

        public ToolbarItemViewModel(ToolbarViewModel toolbar, string controlType, string color, string name) 
        {
            Toolbar = toolbar;
            ControlType = controlType;

            Color = color;
            Name = name;

            //MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            DragOverCommand = new RelayCommand<DragEventArgs>(OnDragOver);
            MouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseLeftButtonUp);
        }

        private void OnDragOver(DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(typeof(WorkspaceItemViewModel)))
            //{
            //    var draggedItem = e.Data.GetData(typeof(WorkspaceItemViewModel)) as WorkspaceItemViewModel;
            //    Point dropPosition = e.GetPosition(e.Source as IInputElement);
            //    if (draggedItem != null)
            //    {
            //        dropPosition.X -= mouseOffset.X;
            //        dropPosition.Y -= mouseOffset.Y;
            //        draggedItem.Position = dropPosition;
            //    }

            //}

        }

        private void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel selectedItem = WorkspaceItemCreator.GetWorkspaceItem(ControlType, mousePos);
            selectedItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(selectedItem);
        }

        //private void OnMouseMove(MouseEventArgs e)
        //{
        //    Point mousePos = e.GetPosition(null);
        //    if (Toolbar.Builder.WorkspaceItemSelected.Count > 0)
        //        Toolbar.Builder.WorkspaceItemSelected[0].Position = mousePos;
        //}

        private void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Toolbar.Builder.WorkspaceItemSelected.Clear();
        }
    }
}
