using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Builder.ViewModels.WorkspaceElements
{
     class WSFlattenViewModel: WorkspaceItemViewModel
    {
        [JsonIgnore]
        public override string DisplayName =>
            $"{Name}";

        public WSFlattenViewModel(double x, double y, int width, int height, double opacity = 1, string name = "")
            : base(x, y, width, height, opacity, name)
        {
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons", "Flatten.png");
        }
    }
}
