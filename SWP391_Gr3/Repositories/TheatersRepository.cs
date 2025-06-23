using Microsoft.EntityFrameworkCore;
using MimeKit;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public class TheatersRepository : ITheatersRepository
    {
        private readonly Swp391Context _context;

        public TheatersRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<bool> AddTheaterAsync(Theater theater)
        {
            _context.Theaters.Add(theater);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Theater>> GetAllTheaterssAsync()
        {
            return await _context.Theaters.ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync(int Thid)
        {
            return await _context.Rooms.Where(c => c.TheaterId == Thid).Include(c => c.Theater).ToListAsync();
        }

        public async Task<bool> AddRoomAsync(int id, Room room)
        {
            var room1 = new Room
            {
                Code = room.Code,
                TheaterId = id,
            };
            _context.Rooms.Add(room1);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RoomCodeExistsAsync(string code, int theaterId)
        {
            return await _context.Rooms.AnyAsync(r => r.Code == code && r.TheaterId == theaterId);
        }
    }
}
