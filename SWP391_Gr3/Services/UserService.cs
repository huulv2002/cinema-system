using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;

namespace SWP391_Gr3.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepo.GetUserByIdAsync(userId);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            var validateEmail = await _userRepo.GetUserByEmailAsync(user.Email);
            if (validateEmail == null)
            {
                return await _userRepo.AddUserAsync(user);
            }
            else
            {
                return false;
            }
        }


        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            return (user != null && user.HashPass == password) ? user : null;
        }

        public async Task<string?> GetRoleNameAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.");
            }

            var roleName = await _userRepo.GetRoleNameAsync(userId);
            return roleName ?? "Unknown Role";
        }
        public async Task<User?> GetUserById(int userId)
        {
            return await _userRepo.GetUserById(userId);
        }
        public async Task<bool> UpdateProfile(User user)
        {
            return await _userRepo.UpdateProfile(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepo.GetUserByEmailAsync(email);
        }

        public async Task<bool> UpdateVerification(User user)
        {
            return await _userRepo.UpdateVerification(user);
        }

        public async Task<bool> UpdatePassword(string email, String password)
        {
            return await _userRepo.UpdatePassword(email, password);
        }
        public async Task<bool> ValidateUser(string email, string password)
        {
            return await _userRepo.ValidateUser(email, password);
        }

        public async Task<bool> ToggleUserActiveStatusAsync(int userId)
        {
            return await _userRepo.ToggleUserActiveStatusAsync(userId);
        }
    }
}
