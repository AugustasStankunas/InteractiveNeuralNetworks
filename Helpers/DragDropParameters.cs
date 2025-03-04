using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace InteractiveNeuralNetworks.Helpers
{
    public class DragDropParameters
    {
        public DragEventArgs EventArgs { get; set; }
        public Canvas ReferenceCanvas { get; set; }
    }
}
