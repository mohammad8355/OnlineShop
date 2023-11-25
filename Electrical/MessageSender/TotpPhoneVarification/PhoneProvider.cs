using Microsoft.Extensions.Options;
using OtpNet;
using System.Drawing.Imaging;
using System.Text;

namespace PresentationLayer.MessageSender.TotpPhoneVarification
{
    public class PhoneProvider : IPhoneProvider
    {
        private Totp Totp;
        private readonly TotpPhoneProviderOptions options;
        public PhoneProvider(IOptions<TotpPhoneProviderOptions> _options)
        {
            options = _options?.Value ?? new TotpPhoneProviderOptions();
        }

        public string GenerateTotpCode(string secretKey)
        {
            createTotp(secretKey);
            return Totp.ComputeTotp();
        }

        public PhoneProviderResualt VarifyCode(string secretKey, string TotpCode)
        {
            createTotp(secretKey);
          var isTotpValid = Totp.VerifyTotp(TotpCode,out _);
            if(isTotpValid)
            {
                return new PhoneProviderResualt() { Succeeded = true, };
            }
            else
            {
                return new PhoneProviderResualt() { Succeeded = false, ErrorMessage = "کد وارد شده نامعتبر است" };
            }
        }
        private void createTotp(string secretKey)
        {
            Totp = new Totp(Encoding.UTF8.GetBytes(secretKey),options.StepInSecond,OtpHashMode.Sha1,options.TotpSize);
        }
    }
}
