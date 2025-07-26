using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Models;

public partial class ProductCategory
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(50, ErrorMessage = "Tên tối đa 50 ký tự")]
    [RegularExpression(@"\S.*", ErrorMessage = "Không được chỉ chứa khoảng trắng")]
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
