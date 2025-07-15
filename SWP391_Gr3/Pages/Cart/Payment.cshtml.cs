using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SWP391_Gr3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        private readonly Swp391Context _context;

        public PaymentModel(Swp391Context context) => _context = context;

        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }

        [BindProperty]
        public int? SelectedPromotionId { get; set; }

        [BindProperty]
        public bool IsPaymentConfirmed { get; set; }

        public List<SelectListItem> AvailablePromotions { get; set; } = new();

        public Order Order { get; set; }
        public Payment Payment { get; set; }
        public string QrImage { get; set; }
        public bool IsExpired { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public bool ShowSuccessPopup { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadOrderData();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            await LoadOrderData();

            if (Order == null || Payment == null)
                return NotFound();

            // Nếu đơn hết hạn, redirect về Cart
            if (Order.CreatedAt.AddMinutes(30) < DateTime.Now)
                return RedirectToPage("/Cart/Index");

            if (action == "confirmPayment")
            {
                // Áp dụng khuyến mãi được chọn
                Order.PromotionId = SelectedPromotionId;
                await _context.SaveChangesAsync();

                IsPaymentConfirmed = true;

                // Lấy lại khuyến mãi để tính tổng
                await LoadOrderData(); // Gọi lại để cập nhật Order.Promotion mới

                // Tạo QR
                string qrContent = $"Đơn hàng #{Order.Id}\nTổng: {TotalAfterDiscount:N0} đ\nTrạng thái: {Payment.Status}";
                using var qrGen = new QRCodeGenerator();
                using var data = qrGen.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(data);
                var qrBytes = qrCode.GetGraphic(20);
                QrImage = "data:image/png;base64," + Convert.ToBase64String(qrBytes);
            }

            else if (action == "markPaid")
            {
                Payment.Status = "Success";
                await _context.SaveChangesAsync();
                ShowSuccessPopup = true;
            }

            return Page();
        }

        private async Task LoadOrderData()
        {
            Order = await _context.Orders
                .Include(o => o.Payment)
                .Include(o => o.Promotion)
                .FirstOrDefaultAsync(o => o.Id == OrderId);

            if (Order != null)
            {
                Payment = Order.Payment;

                // Tính tổng sau giảm
                decimal discount = Order.Promotion?.Value ?? 0;
                TotalAfterDiscount = Payment?.Amount ?? 0;
                TotalAfterDiscount -= discount;

                // Check hết hạn
                IsExpired = DateTime.Now > Order.CreatedAt.AddMinutes(30);

                // Nạp danh sách khuyến mãi
                AvailablePromotions = await _context.Promotions
                .Where(p => p.IsActive == true)
                .Select(p => new SelectListItem
                  {
                     Value = p.Id.ToString(),
                    Text = $"{p.Code} - Giảm {p.Value:N0} đ"
                  })
                .ToListAsync();

            }
        }
    }
}
