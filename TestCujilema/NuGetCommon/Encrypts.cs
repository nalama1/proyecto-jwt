using System.Security.Cryptography;
using System.Text;

namespace NuGetCommon
{
    public class Encrypts
    {
        public static string GetSHA256(string str)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}
