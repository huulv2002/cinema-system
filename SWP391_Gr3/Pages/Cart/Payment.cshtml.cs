using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X9;
using SWP391_Gr3.Models;
using System.Security.Cryptography;
using System.Text;


namespace SWP391_Gr3.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        private readonly VnPayConfig _vnpayConfig;
        private readonly Swp391Context _context;

        public PaymentModel(Swp391Context context, IOptions<VnPayConfig> vnpayConfig)
        {
            _context = context;
            _vnpayConfig = vnpayConfig.Value;
        }

        public string TransactionStatus { get; set; }
        [BindProperty]
        public int Amount { get; set; }

        [BindProperty]
        public string OrderId { get; set; } = string.Empty;

        [BindProperty]
        public string OrderInfo { get; set; } = string.Empty;
        public List<Promotion> Promotions { get; set; } = new();

      

        public IActionResult OnPost(decimal amount, string orderId, string orderInfo)
        {
            var vnpayParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", _vnpayConfig.Version },
                { "vnp_Command", _vnpayConfig.Command },
                { "vnp_TmnCode", _vnpayConfig.TmnCode },
                { "vnp_Amount", ((int)(amount * 100)).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", _vnpayConfig.CurrCode },
                { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
                { "vnp_Locale", _vnpayConfig.Locale },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", _vnpayConfig.ReturnUrl },
                { "vnp_TxnRef", orderId },
                { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") }
            };

            var query = string.Join("&", vnpayParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var hash = GenerateChecksum(query, _vnpayConfig.HashSecret);

            var paymentUrl = $"{_vnpayConfig.BaseUrl}?{query}&vnp_SecureHash={hash}";
            return Redirect(paymentUrl);
        }

        public async Task OnGetAsync(int? orderId, int? amount, string? orderInfo)
        {
            if (orderId != null && amount != null && orderInfo != null)
            {
                OrderId = orderId.ToString();
                Amount = amount.Value;
                OrderInfo = orderInfo;
            }

            var now = DateTime.Now;
            Promotions = await _context.Promotions
                .Where(p => p.IsActive && p.Stock > 0 && p.StartDate <= now && p.EndDate >= now)
                .ToListAsync();
        }

        private string GenerateChecksum(string data, string key)
        {
            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
