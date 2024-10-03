namespace Infrustructure.MessageSender.TotpPhoneVarification
{
    public class TotpPhoneProviderOptions
    {
        public int StepInSecond { get; set; } = 30;
        public int TotpSize { get; set; } = 6;

    }
}
