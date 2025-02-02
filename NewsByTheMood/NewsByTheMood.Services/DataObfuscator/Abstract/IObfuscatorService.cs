namespace NewsByTheMood.Services.DataObfuscator.Abstract
{
    public interface IObfuscatorService
    {
        public string Obfuscate(string plainValue, string? secret = null);
        public string Deobfuscate(string chipervalue, string? secret = null);
    }
}
