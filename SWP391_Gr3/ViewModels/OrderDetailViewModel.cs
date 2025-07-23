namespace SWP391_Gr3.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string MovieTitle { get; set; } = "";
        public DateTime Showtime { get; set; }
        public List<(string TicketCode, string SeatCode, string SeatType, decimal Price)> Tickets { get; set; } = new();


        public List<(string ComboName, int Quantity, decimal Price)> Combos { get; set; } = new();
        public List<(string ProductName, int Quantity, decimal Price)> Products { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
