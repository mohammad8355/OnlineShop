using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Payment
{
    public interface IPayment
    {
        public Task<Dto.Response.Payment.Request> Pay(string MerchantID, string CallbackURL, int Amount, string Description,string Email, string mobile);
        public Task<Dto.Response.Payment.Verification> Verification(int Amount,string Authority,string MerchantID);
    }
}
