namespace NewsByTheMood.Core.Crypto
{
    public static class XORCrypt
    {
        public static byte[] Encrypt(byte[] plaintext, byte[] secret)
        {
            if (plaintext.Count() <= 0) 
                throw new ArgumentNullException("plaintext", "Parameter plaintext cannot be null or empty string");
            if (secret.Count() <= 0) 
                throw new ArgumentNullException("secret", "Parameter secret cannot be null or empty string");

            var rate = Math.Ceiling((decimal)plaintext.Length / secret.Length);
            var result = new byte[plaintext.Length >= secret.Length ? plaintext.Length : secret.Length];
                
            for (var i = result.Length; i > 0; i--)
            {
                result[result.Length - i] = plaintext[i] ^ secret[];
            }

            return (result);
        }
        public static string Decrypt(string chipervalue, string secret)
        {
            if (string.IsNullOrEmpty(chipervalue)) 
                throw new ArgumentNullException(chipervalue, "Parameter chipervalue cannot be null or empty string");
            if (string.IsNullOrEmpty(secret)) 
                throw new ArgumentNullException(secret, "Parameter secret cannot be null or empty string");
            return (Int64.Parse(chipervalue) ^ Int64.Parse(secret)).ToString();
        }
    }
}
