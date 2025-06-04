using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        public string? Otp { get; set; }

        [BindProperty]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string? NewPassword { get; set; }

        [BindProperty]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public bool ShowResetForm { get; set; } = false;

        public async Task<IActionResult> OnPostSendOtpAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userService.GetUserByEmailAsync(Email);
            if (user == null)
            {
                ErrorMessage = "Email không tồn tại trong hệ thống.";
                return Page();
            }

            var otp = new Random().Next(100000, 999999).ToString();
            string subject = "Mã OTP khôi phục mật khẩu";
            string body = $"Mã OTP của bạn là: {otp}";

            try
            {
                await _emailService.SendEmailAsync(Email, subject, body);

                user.VerificationCode = otp;
                user.CodeExpiration = DateTime.Now.AddMinutes(5);
                await _userService.UpdateVerification(user);

                SuccessMessage = "Đã gửi mã OTP đến email của bạn.";
                ShowResetForm = true;
            }
            catch
            {
                ErrorMessage = "Không thể gửi email.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResetPasswordAsync()
        {
            ShowResetForm = true;

            if (!ModelState.IsValid)
                return Page();

            var user = await _userService.GetUserByEmailAsync(Email);
            if (user == null || user.VerificationCode != Otp || user.CodeExpiration < DateTime.Now)
            {
                ErrorMessage = "Mã OTP không đúng hoặc đã hết hạn.";
                return Page();
            }

            var result = await _userService.UpdatePassword(Email, NewPassword!);
            if (!result)
            {
                ErrorMessage = "Không thể cập nhật mật khẩu.";
                return Page();
            }

            SuccessMessage = "Mật khẩu đã được đặt lại thành công.";
            return Page();
        }
    }
}
