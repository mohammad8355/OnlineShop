using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using BusinessLogicLayer.OtpCodes;

namespace PresentationLayer.Areas.Identity.Pages.Account
{
    public class PhoneNumberConfrimModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OtpCodeLogic _otpCodeLogic;

        public PhoneNumberConfrimModel(OtpCodeLogic otpCodeLogic,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _otpCodeLogic = otpCodeLogic;
            _signInManager = signInManager;
        }
        [BindProperty]
        public OtpCodeVerify Input { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        public class OtpCodeVerify
        {
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string phonenumber)
        { 
            PhoneNumber = phonenumber;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if ( PhoneNumber == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByNameAsync(PhoneNumber);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{PhoneNumber}'.");
            }

            var code = await _otpCodeLogic.GetCodeByphoneNumber(PhoneNumber);
            //TODO: i comment code becuase send otp code via sms is not work
            // if (code == Input.Code)
            // {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToPage("Login");    
            // }
            // else
            // {
            //     return Page();
            // }
            
        }
    }
}
