using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public interface IUserRepository
    {
        Task<string?> GetRoleNameAsync(int userId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
