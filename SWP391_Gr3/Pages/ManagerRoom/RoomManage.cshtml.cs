using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.ManagerRoom
{
    [AuthorizeRole("Owner")]
    public class RoomManageModel : PageModel
    {
        private readonly ITheatersService _theatersService;

        public RoomManageModel(ITheatersService theatersService)
        {
            _theatersService = theatersService;
        }
        public IEnumerable<Room> Rooms { get; set; } = new List<Room>();

        public int TheaterId { get; set; }
        public string TheaterName { get; set; }
        public async Task OnGetAsync(int id,string name)
        {
            TheaterId = id;
            TheaterName=name;
            Rooms = await _theatersService.ListAllRoomrAsync(id);
        }
    }
}
