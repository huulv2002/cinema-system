using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Dtos
{
    public class ComboDto
    {
        public int Id { get; set; } // Dùng cho Edit, không cần trong Create nhưng có thể giữ

        [Required(ErrorMessage = "Tiêu đề không được để trống.")]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0.01, 100000000, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Mô tả  không được để trống.")]

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int? TheaterId { get; set; }
    }
}
