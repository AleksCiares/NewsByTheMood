using System.Text;
using NewsByTheMood.Services.DataObfuscator.Abstract;

namespace NewsByTheMood.Services.DataObfuscator.Implement
{
    public class VerySimpleObfuscatorService : IObfuscatorService
    {
        public string Obfuscate(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException("plaintext", "Parameter plaintext cannot be null or empty string");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plaintext));
        }
        public string Deobfuscate(string chipervalue)
        {
            if (string.IsNullOrEmpty(chipervalue))
                throw new ArgumentNullException("plaintext", "Parameter plaintext cannot be null or empty string");

            return Encoding.UTF8.GetString(Convert.FromBase64String(chipervalue));
        }
    }
}
