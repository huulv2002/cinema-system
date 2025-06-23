namespace SWP391_Gr3.ViewModels
{
    public class MovieReviewHomeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? ImageUrl { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public string? Summary { get; set; }
    }

}
