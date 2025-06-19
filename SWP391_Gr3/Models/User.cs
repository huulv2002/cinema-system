using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? HashPass { get; set; }

    public int? RoleId { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public int? TheaterId { get; set; }

    public string? VerificationCode { get; set; }

    public DateTime? CodeExpiration { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }

    public virtual Theater? Theater { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
