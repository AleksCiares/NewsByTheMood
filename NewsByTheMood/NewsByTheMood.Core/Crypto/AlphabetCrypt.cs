using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCoreSampleApp
{
    public sealed class AlphabetCrypt
    {
        private readonly string _secret;

        public AlphabetCrypt(string secret)
        {
            if (secret.Length < 12)
            {
                throw new ArgumentException("AlphabetCrypt. Secret must be longer than 12 characters.");
            }

            var temp = secret;
            var symbol = ' ';
            do 
            {
                symbol = temp[0];
                temp = temp.Remove(0, 1);
                if (temp.Contains(symbol))
                {
                    throw new ArgumentException ("AlphabetCrypt. The secret must not contain repeated characters..");
                }
            } while (temp.Length > 0);
            this._secret = secret;
        }

        public string Obfuscate(string plaintext)
        {
            List<char> encryptedText = new List<char>();
            int rate = this._secret.Length-1;
            int divisionWhole = 0;
            int divisionRemainder = 0;
            int symbolCode = 0;

            //encryptedText.Add(this._secret[0]);
            foreach (char item in plaintext)
            {
                symbolCode = item;
                if (symbolCode % rate == 0)
                {
                    for (int i = 0; i < symbolCode / rate; i++) 
                        encryptedText.Add(this._secret[rate]);
                    encryptedText.Add(this._secret[0]);
                    continue;
                }
                do
                {
                    divisionWhole = symbolCode / rate;
                    divisionRemainder = symbolCode % rate;
                    encryptedText.Add(this._secret[divisionRemainder]);
                    symbolCode = divisionWhole;
                } while (divisionRemainder != 0) ;
            }
            encryptedText.RemoveAt(encryptedText.Count - 1);

            return new string(encryptedText.ToArray());
        }

        public string Deobfuscate(string chipertext)
        {
            var chipertextArray = chipertext.ToCharArray();
            int rate = this._secret.Length - 1;
            List<char> plainText = new List<char>();
            int plainCode = this._secret.IndexOf(chipertext[chipertextArray.Length - 1]);

            for (int i = chipertextArray.Length - 1; i >= 0;  i--)
            {
                if (i - 1 < 0)
                {
                    plainText.Add((char)plainCode);
                    break;
                }
                if (this._secret.IndexOf(chipertextArray[i - 1]) == 0)
                {
                    plainText.Add((char)plainCode);
                    plainCode = 0;
                    continue;
                }
                if (this._secret.IndexOf(chipertextArray[i - 1]) % rate == 0)
                {
                    int temp = 0;
                    do 
                    {
                        i--;
                        temp += this._secret.IndexOf(chipertextArray[i]);
                        if (i - 1 < 0)
                        {
                            break;
                        }
                    } while (this._secret.IndexOf(chipertextArray[i - 1]) != 0);
                    plainText.Add((char)temp);
                    continue;
                }
                plainCode = rate * plainCode + this._secret.IndexOf(chipertextArray[i - 1]);
            }

            plainText.Reverse();
            return new string(plainText.ToArray());
        }

    }
}
