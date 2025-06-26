using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Size không được để trống")]
    [StringLength(20, ErrorMessage = "Size tối đa 20 ký tự")]
    public string Size { get; set; }

    [Required(ErrorMessage = "Giá không được để trống")]
    [Range(1000, 100000000, ErrorMessage = "Giá phải từ 1.000đ đến 100 triệu")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Tồn kho không được để trống")]
    [Range(0, 10000, ErrorMessage = "Tồn kho phải từ 0 đến 10.000")]
    public int Stock { get; set; }


    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    public int ProductCategoryId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ProductCategory? ProductCategory { get; set; }

    public virtual ICollection<ProductCombo> ProductCombos { get; set; } = new List<ProductCombo>();
}
