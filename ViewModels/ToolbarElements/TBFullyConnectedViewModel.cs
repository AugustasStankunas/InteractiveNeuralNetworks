﻿using System.Windows.Input;
using System.Windows;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;
using System.IO;


namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBFullyConnectedViewModel : ToolbarItemViewModel
    {
        public TBFullyConnectedViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Fully Connected layer";
            WorkspaceItem = new WSFullyConnectedViewModel(128, 128, 0, 0, 60, 60);
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
			IsSelected = true;
			Point mousePos = e.GetPosition(null);
            WorkspaceItemViewModel workspaceItem = new WSFullyConnectedViewModel(128, 128, mousePos.X, mousePos.Y, 60, 60);
            workspaceItem.Opacity = 0.5;
            Toolbar.Builder.WorkspaceItemSelected.Add(workspaceItem);
        }
    }
}
