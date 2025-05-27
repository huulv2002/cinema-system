using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;

namespace SWP391_Gr3.Services
{
    public class RoleSer : IRoleSer
    {
        private readonly IRoleRepo _roleRepo;
        public RoleSer(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _roleRepo.GetAllRoleAsync();
        }
    }
}
