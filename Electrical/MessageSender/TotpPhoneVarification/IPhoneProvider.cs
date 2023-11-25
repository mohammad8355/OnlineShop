namespace PresentationLayer.MessageSender.TotpPhoneVarification
{
    public interface IPhoneProvider
    {
        public string GenerateTotpCode(string secretKey);
        public PhoneProviderResualt VarifyCode(string secretKey,string TotpCode);
    }
}
