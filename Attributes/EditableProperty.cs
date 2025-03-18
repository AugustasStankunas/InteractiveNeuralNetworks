using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveNeuralNetworks.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class EditableProperty : Attribute 
    { 
        public string ControlType { get; set; }
        public EditableProperty(string controlType = "TextBox")
        {
            ControlType = controlType;
        }
    }
}
