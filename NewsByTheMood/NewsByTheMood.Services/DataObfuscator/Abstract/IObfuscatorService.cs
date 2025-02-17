namespace NewsByTheMood.Services.DataObfuscator.Abstract
{
    public interface IObfuscatorService
    {
        public string Obfuscate(string plainValue);
        public string Deobfuscate(string chipervalue);
    }
}
