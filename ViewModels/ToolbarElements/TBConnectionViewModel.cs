﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using InteractiveNeuralNetworks.ViewModels.WorkspaceElements;

namespace InteractiveNeuralNetworks.ViewModels.ToolbarElements
{
    class TBConnectionViewModel : ToolbarItemViewModel
    {
        public TBConnectionViewModel(ToolbarViewModel toolbar) : base(toolbar)
        {
            Name = "Connection";
        }

        public override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
			IsSelected = true;
			Toolbar.Builder.isMakingConnection = true;
            Toolbar.Builder.connectionInProgress = new WSConnectionViewModel();
        }
    }
}
