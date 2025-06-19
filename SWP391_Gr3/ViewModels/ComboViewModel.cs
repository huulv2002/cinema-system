namespace SWP391_Gr3.ViewModels
{
    public class ComboViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public List<ComboProductItem> Products { get; set; } = new();
    }
}
