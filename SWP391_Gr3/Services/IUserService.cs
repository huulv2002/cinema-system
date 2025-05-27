using SWP391_Gr3.Models;

namespace SWP391_Gr3.Services
{
    public interface IUserService
    {
        Task<string?> GetRoleNameAsync(int userId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> RegisterUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<User?> ValidateUserAsync(string email, string password);
    }
}
