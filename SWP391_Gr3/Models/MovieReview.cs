using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Models;

public partial class MovieReview
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    [MinLength(3, ErrorMessage = "Tiêu đề phải có ít nhất 3 ký tự.")]
    [StringLength(100, ErrorMessage = "Tiêu đề tối đa 100 ký tự.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Tóm tắt không được để trống.")]
    [MinLength(10, ErrorMessage = "Tóm tắt phải có ít nhất 10 ký tự.")]
    [StringLength(300, ErrorMessage = "Tóm tắt tối đa 300 ký tự.")]
    public string? Summary { get; set; }

    [Required(ErrorMessage = "Nội dung không được để trống.")]
    [MinLength(20, ErrorMessage = "Nội dung phải có ít nhất 20 ký tự.")]
    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime PublishedDate { get; set; }

    public int Likes { get; set; }

    public int Views { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn một bộ phim.")]
    public int MovieId { get; set; }

    public virtual Movie? Movie { get; set; }
}
