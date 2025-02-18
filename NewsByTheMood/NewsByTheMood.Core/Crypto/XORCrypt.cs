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

            //var rate = Math.Ceiling((decimal)plaintext.Length / secret.Length);
               
            for (var i = 0; i < plaintext.Length; i++)
            {
                plaintext[i] = (byte)(plaintext[i] ^ secret[(i % secret.Length)]);
            }

            return (plaintext);
        }
        public static byte[] Decrypt(byte[] chipervalue, byte[] secret)
        {
            if (chipervalue.Count() <= 0)
                throw new ArgumentNullException("plaintext", "Parameter chipervalue cannot be null or empty string");
            if (secret.Count() <= 0)
                throw new ArgumentNullException("secret", "Parameter secret cannot be null or empty string");

            for (var i = 0; i < chipervalue.Length; i++)
            {
                chipervalue[i] = (byte)(chipervalue[i] ^ secret[(i % secret.Length)]);
            }

            return (chipervalue);
        }
    }
}
