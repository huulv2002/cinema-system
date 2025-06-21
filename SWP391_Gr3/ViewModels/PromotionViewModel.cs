namespace SWP391_Gr3.ViewModels
{
    public class PromotionViewModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public int PromotionTypeId { get; set; }

        public decimal? Value { get; set; }

        public bool IsActive { get; set; } = false;

        public int? Stock { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}
