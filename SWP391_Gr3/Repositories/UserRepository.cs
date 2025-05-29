using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Swp391Context _context;

        public UserRepository(Swp391Context context)
        {
            _context = context;
        }
        //add
        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }
        //delete
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
            if (user == null)
            {
                throw new Exception("not found");
            }
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        //list
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }
        //findbyemail
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        //findbyid
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
        }
        //update
        public async Task<bool> UpdateUserAsync(User user)
        {

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false; // Không tìm thấy user
            }


            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.HashPass = user.HashPass;
            existingUser.Address = user.Address;
            existingUser.RoleId = user.RoleId;


            return await _context.SaveChangesAsync() > 0;
        }
        //getRoleName

        public async Task<string?> GetRoleNameAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Role?.Name;
        }
        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
        }
        public async Task<bool> UpdateProfile(User user)
        {

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false; 
            }


            existingUser.FullName = user.FullName;
            
            existingUser.PhoneNumber = user.PhoneNumber;

            existingUser.Address = user.Address;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateVerification(User user)
        {

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }


            existingUser.CodeExpiration = user.CodeExpiration;

            existingUser.VerificationCode = user.VerificationCode;

        

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdatePassword(string email, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;


            user.HashPass = password;

            user.VerificationCode = null;
            user.CodeExpiration = null;

           return await _context.SaveChangesAsync() > 0;
        }

       
    }
}
