using SWP391_Gr3.Models;

namespace SWP391_Gr3.Services
{
    public interface IUserService
    {
        Task<bool> UpdateUserRoleAsync(int userId, int roleId); // updating user role
        Task<bool> ToggleUserActiveStatusAsync(int userId); // active
        Task<string?> GetRoleNameAsync(int userId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> RegisterUserAsync(User user);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> GetUserById(int userId);
        Task<bool> UpdateProfile(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateVerification(User user);
        Task<bool> UpdatePassword(string email, string password);
        Task<bool> ValidateUser(string email, string password);
    }
}
