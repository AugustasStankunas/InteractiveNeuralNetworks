using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Builder.ViewModels.WorkspaceElements
{
    class WSDropoutViewModel: WorkspaceItemViewModel
    {
        private double _rate;
        [Attributes.EditableProperty]
        public double Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged(nameof(Rate));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}\nR:{Rate}";

        public WSDropoutViewModel(double rate, double x, double y, int width, int height, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            Rate = rate;
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Dropout.png");
        }
    }
}
