using Microsoft.EntityFrameworkCore;
using MimeKit;
using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

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

        public async Task<bool> ToggleTheaterActiveStatusAsync(int theaterId)
        {
            var theater = await _context.Theaters.FindAsync(theaterId);
            if (theater == null)
            {
                return false;
            }
            theater.IsActive = !theater.IsActive;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OperationResult> DeleteTheaterAsync(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Không tìm thấy rạp chiếu phim."
                };
            }
            var hasRooms = await _context.Rooms.AnyAsync(r => r.TheaterId == id);
            var hasUsers = await _context.Users.AnyAsync(u => u.TheaterId == id);
            if (hasUsers)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Rạp đang có người dùng không thể xóa."
                };
            }
            if (hasRooms)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Không thể xóa rạp vì vẫn còn phòng liên quan."
                };
            }
            else
            {
                _context.Theaters.Remove(theater);
                var result = await _context.SaveChangesAsync() > 0;
                return new OperationResult
                {
                    Success = result,
                    Message = result ? "Xóa rạp chiếu phim thành công." : "Xóa rạp chiếu phim thất bại."
                };
            }
        }
    }
}
