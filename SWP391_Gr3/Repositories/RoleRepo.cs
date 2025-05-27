using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public class RoleRepo : IRoleRepo
    {
        private readonly Swp391Context _context;
        public RoleRepo(Swp391Context context)
        {
            _context = context;
        }
        //list
        public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
