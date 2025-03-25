using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSAddViewModel: WorkspaceItemViewModel
    {
        private int _numInputs;
        [Attributes.EditableProperty]
        public int NumInputs
        {
            get => _numInputs;
            set
            {
                _numInputs = value;
                OnPropertyChanged(nameof(NumInputs));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nN:{NumInputs}";

        public WSAddViewModel(int numInputs, double x, double y, int width = 60, int height = 60, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            NumInputs = numInputs;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Add.png");
        }
    }
}
