using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Models;

public partial class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string? Email { get; set; }

    public string? HashPass { get; set; }

    public int? RoleId { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [RegularExpression(@"^(03|07|08|09)\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    
    public string? PhoneNumber { get; set; }
    
    
    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public int? TheaterId { get; set; }

    public string? VerificationCode { get; set; }

    public DateTime? CodeExpiration { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }

    public virtual Theater? Theater { get; set; }
}
