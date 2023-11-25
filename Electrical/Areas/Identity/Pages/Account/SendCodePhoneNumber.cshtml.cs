using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PresentationLayer.MessageSender.TotpPhoneVarification;

namespace PresentationLayer.Areas.Identity.Pages.Account
{
    public class SendCodePhoneNumberModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IPhoneProvider phoneProvider;
        public SendCodePhoneNumberModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IPhoneProvider phoneProvider)
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
