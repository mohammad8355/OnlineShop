using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrustructure.MessageSender.TotpPhoneVarification;
namespace PresentationLayer.Areas.Identity.Pages.Account
{
    public class SendCodePhoneNumberModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private IPhoneProvider phoneProvider;
        public SendCodePhoneNumberModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IPhoneProvider phoneProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.phoneProvider = phoneProvider;
        }
        public PhoneNumberCode Input { get; set; }
        [BindProperties]
        public class PhoneNumberCode
        {
            public string Code { get; set; }
        }
        public IActionResult OnGet()
        {
            var secretKey = new Guid();
            phoneProvider.GenerateTotpCode(secretKey.ToString());
            return Page();
        }
        public void OnPost()
        {

         
           
        }
    }
}
