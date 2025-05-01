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
