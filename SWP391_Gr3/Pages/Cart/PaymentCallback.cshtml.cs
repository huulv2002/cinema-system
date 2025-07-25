using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.VnPayLib;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SWP391_Gr3.Pages.Cart
{
    public class PaymentCallbackModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly VnPayConfig _vnpayConfig;

        public PaymentCallbackModel(Swp391Context context, Microsoft.Extensions.Options.IOptions<VnPayConfig> config)
        {
            _context = context;
            _vnpayConfig = config.Value;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            var vnp_TxnRef = Request.Query["vnp_TxnRef"];
            var vnp_Amount = Request.Query["vnp_Amount"];
            var vnp_TransactionNo = Request.Query["vnp_TransactionNo"];
            var vnp_SecureHash = Request.Query["vnp_SecureHash"];
            var vnp_SecureHashType = Request.Query["vnp_SecureHashType"];

            // Chuẩn bị xác thực chữ ký
            var vnpayData = new SortedDictionary<string, string>();
            foreach (var key in Request.Query.Keys)
            {
                if (key.StartsWith("vnp_") && key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    vnpayData.Add(key, Request.Query[key]);
                }
            }

            // Tạo chuỗi để xác minh
            var dataString = string.Join("&", vnpayData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var checkHash = GenerateChecksum(dataString, _vnpayConfig.HashSecret);

            bool isSuccess = false;
            string transactionStatus = "Thất bại";

            if (checkHash == vnp_SecureHash && vnp_ResponseCode == "00")
            {
                isSuccess = true;
                transactionStatus = "Thành công";

                int orderId = int.Parse(vnp_TxnRef);

                var order = await _context.Orders
                    .Include(o => o.Tickets)
                    .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null)
                {
                    if (order != null && order.PaymentId.HasValue)
                    {
                        var payment = await _context.Payments.FindAsync(order.PaymentId.Value);
                        if (payment != null)
                        {
                            payment.Code = vnp_TransactionNo;
                            payment.Status = "Success";
                            payment.CreatedAt = DateTime.Now;
                            payment.Amount = decimal.Parse(vnp_Amount) / 100;
                        }

                        order.IsConfirmed = true;

                        // Trừ tồn kho như cũ
                        foreach (var op in order.OrderProducts)
                        {
                            if (op.Product != null)
                            {
                                op.Product.Stock -= op.Quantity ?? 0;
                                if (op.Product.Stock < 0)
                                    op.Product.Stock = 0;
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    order.IsConfirmed = true;

                    // ✅ TRỪ TỒN KHO CỦA SẢN PHẨM SAU THANH TOÁN
                    foreach (var op in order.OrderProducts)
                    {
                        if (op.Product != null)
                        {
                            op.Product.Stock -= op.Quantity ?? 0;
                            if (op.Product.Stock < 0)
                                op.Product.Stock = 0;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["OrderId"] = vnp_TxnRef;
            TempData["Amount"] = (decimal.Parse(vnp_Amount) / 100).ToString("N0");
            TempData["TransactionStatus"] = transactionStatus;

            return Page();
        }

        private string GenerateChecksum(string data, string key)
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
