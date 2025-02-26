namespace NewsByTheMood.Core.Crypto
{
    public static class XORCrypt
    {
        public static byte[] Encrypt(byte[] plaintext, byte[] secret)
        {
            if (plaintext.Count() <= 0) 
                throw new ArgumentNullException("XORCrypt. Parameter plaintext cannot be empty byte array");
            if (secret.Count() <= 0) 
                throw new ArgumentNullException("XORCrypt. Parameter secret cannot be empty byte array");
               
            for (var i = 0; i < plaintext.Length; i++)
            {
                plaintext[i] = (byte)(plaintext[i] ^ secret[(i % secret.Length)]);
            }

            return plaintext;
        }
        public static byte[] Decrypt(byte[] chipervalue, byte[] secret)
        {
            if (chipervalue.Count() <= 0)
                throw new ArgumentNullException("XORCrypt. Parameter chipervalue cannot be empty byte array");
            if (secret.Count() <= 0)
                throw new ArgumentNullException("XORCrypt. Parameter secret cannot be empty byte array");

            for (var i = 0; i < chipervalue.Length; i++)
            {
                chipervalue[i] = (byte)(chipervalue[i] ^ secret[(i % secret.Length)]);
            }

            return chipervalue;
        }
    }
}
