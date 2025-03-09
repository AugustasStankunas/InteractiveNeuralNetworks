using InteractiveNeuralNetworks.ViewModels;
using InteractiveNeuralNetworks.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteractiveNeuralNetworks.Helpers
{
    public static class WorkspaceItemCreator
    {
        public static WorkspaceItemViewModel GetWorkspaceItem(string type, Point pos)
        {
            switch (type)
            {
                case "OrangeControl":
                    return new WorkspaceItemViewModel("OrangeControl", pos.X, pos.Y, 60, 60, "Orange");
                case "PinkControl":
                    return new WorkspaceItemViewModel("PinkControl", pos.X, pos.Y, 50, 50, "Pink");
                case "BlueControl":
                    return new WorkspaceItemViewModel("BlueControl", pos.X, pos.Y, 75, 75, "LightBlue");
            }

            return new WorkspaceItemViewModel();
        }
    }
}
