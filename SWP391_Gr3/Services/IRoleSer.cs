using SWP391_Gr3.Models;

namespace SWP391_Gr3.Services
{
    public interface IRoleSer
    {
        Task<IEnumerable<Role>> GetAllRoleAsync();
    }
}
