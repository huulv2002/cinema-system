using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SWP391_Gr3.VnPayLib
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> requestData = new(StringComparer.Ordinal);

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                requestData.Add(key, value);
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var queryString = new StringBuilder();
            foreach (var item in requestData)
            {
                queryString.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value)}&");
            }

            var rawData = queryString.ToString().TrimEnd('&');
            var secureHash = HmacSHA512(hashSecret, rawData);

            return $"{baseUrl}?{rawData}&vnp_SecureHash={secureHash}";
        }

        private string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);

            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
