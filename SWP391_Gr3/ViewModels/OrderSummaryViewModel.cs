namespace SWP391_Gr3.ViewModels
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public string MovieTitle { get; set; } = "";
        public DateTime ShowtimeDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
