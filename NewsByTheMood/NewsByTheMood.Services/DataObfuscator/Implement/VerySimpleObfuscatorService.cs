using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Services.DataObfuscator.Abstract;

namespace NewsByTheMood.Services.DataObfuscator.Implement
{
    public class VerySimpleObfuscatorService : IObfuscatorService
    {
        // its temporary value, will replace to constructor and configure
        private readonly string _obfuscatorSecret = "64486";
        public string Obfuscate(string plainValue, string? secret = null)
        {
            if (secret == null) secret = _obfuscatorSecret;
            return (Int64.Parse(plainValue) ^ Int64.Parse(secret)).ToString();
        }
        public string Deobfuscate(string chipervalue, string? secret = null)
        {
            if (secret == null) secret = _obfuscatorSecret;
            return (Int64.Parse(chipervalue) ^ Int64.Parse(secret)).ToString();
        }
    }
}
