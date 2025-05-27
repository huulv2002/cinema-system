using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.ViewModels
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string HashPass { get; set; }
        public string? PhoneNumber { get; set; }

        public bool? isActive { get; set; } = true;

        public string address { get; set; }

        public int role { get; set; } = 2;
    }
}
