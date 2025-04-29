using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train.Helpers
{
    public class AugmentationItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public List<AugmentationItem> Children { get; set; }

        public AugmentationItem()
        {
            Children = new List<AugmentationItem>();
        }
    }
}
