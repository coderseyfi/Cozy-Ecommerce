using Cozy.Domain.AppCode.Extensions;
using Microsoft.Extensions.Options;

namespace Cozy.Domain.AppCode.Services
{
    public class CryptoService
    {
        private readonly CryptoServiceOptions options;

        public CryptoService(IOptions<CryptoServiceOptions> options)
        {
            this.options = options.Value;
        }

        public string Encrypt(string value, bool appliedUrlEncode = false)
        {
            return value.Encrypt(options.SymmetricKey, appliedUrlEncode);
        }

        public string Decrypt(string value)
        {
            return value.Decrypt(options.SymmetricKey);
        }
    }


    public class CryptoServiceOptions
    {
        public string SaltKey { get; set; }
        public string SymmetricKey { get; set; }
    }
}
