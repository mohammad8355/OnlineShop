using Dto.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarinPal;
using ZarinPal.Class;

namespace Infrustructure.Payment
{
    public class ZarinPalPay : IPayment
    {
        private readonly Authority _authority;
        private readonly ZarinPal.Class.Payment _payment;
        private readonly Transactions _transactions;
        public ZarinPalPay()
        {
            var expose = new Expose();
            _authority = expose.CreateAuthority();
            _payment = expose.CreatePayment();
            _transactions = expose.CreateTransactions();
        }
        public async Task<Dto.Response.Payment.Request> Pay(string MerchantID, string CallbackURL,int Amount, string Description,string Email,string mobile)
        {
            var requestToPaymentGate = await _payment.Request(new DtoRequest() { 
            Amount = Amount,
            CallbackUrl = CallbackURL,
            Description = Description,
            Email = Email,
            Mobile = mobile,
            MerchantId = MerchantID,
            },ZarinPal.Class.Payment.Mode.sandbox);
            return requestToPaymentGate;
        }

        public async Task<Dto.Response.Payment.Verification> Verification(int Amount, string Authority, string MerchantID)
        {
            var verification = await _payment.Verification(new DtoVerification()
            {
                Amount = Amount,
                Authority = Authority,
                MerchantId = MerchantID,
            }, ZarinPal.Class.Payment.Mode.sandbox);
            return verification;
        }
    }
}
