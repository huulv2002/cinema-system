using SWP391_Gr3.Models;

namespace SWP391_Gr3.ViewModels
{
    public class TheaterViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public bool? IsActive { get; set; }=true;
       
    }
}
