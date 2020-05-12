namespace Portfolio.Shared.DataModels
{
    public class Link
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public bool FragmentRoute { get; set; } = false;
    }
}
