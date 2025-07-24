using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using SWP391_Gr3.Models;

public class VnPayIPNModel : PageModel
{
    private readonly VnPayConfig _vnpayConfig;

    public VnPayIPNModel(IOptions<VnPayConfig> vnpayConfig)
    {
        _vnpayConfig = vnpayConfig.Value;
    }

    public IActionResult OnPost()
    {
        var vnpayData = Request.Query;
        var secureHash = vnpayData["vnp_SecureHash"].ToString();
        var sortedParams = new SortedDictionary<string, string>();

        foreach (var key in vnpayData.Keys)
        {
            if (key.StartsWith("vnp_") && key != "vnp_SecureHash")
            {
                sortedParams.Add(key, vnpayData[key]);
            }
        }

        var queryString = string.Join("&", sortedParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        var computedHash = GenerateChecksum(queryString, _vnpayConfig.HashSecret);

        var response = new { RspCode = "97", Message = "Checksum failed" };
        if (secureHash.Equals(computedHash, StringComparison.InvariantCultureIgnoreCase))
        {
            if (vnpayData["vnp_ResponseCode"] == "00")
            {
                // Cập nhật trạng thái giao dịch vào database
                response = new { RspCode = "00", Message = "Confirm Success" };
            }
            else
            {
                response = new { RspCode = "01", Message = "Transaction Failed" };
            }
        }

        return new JsonResult(response);
    }

    private string GenerateChecksum(string data, string hashSecret)
    {
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hashSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}