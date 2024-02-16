// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Electrical.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        //[ViewData]
        //public string TwoStepAuth { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            [DisplayName(displayName: "شماره موبایل")]
            public string PhoneNumber { get; set; }
            [Required]
            [MaxLength(100)]
            [Display(Name = "نام کاربری")]
            public string Username { get; set; }
            [MaxLength(20)]
            [MinLength(3)]
            [DisplayName(displayName: "نام و نام خانوداگی")]
            public string FullName { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayName(displayName:"تاریخ تولد")]
            public DateTime birthDate { get; set; }
            [MaxLength(800)]
            [DisplayName(displayName: "آدرس")]
            public string Address { get; set; }
            [MaxLength(10)]
            [DisplayName(displayName: "کد پستی")]
            [RegularExpression("^[0-9]*$",ErrorMessage ="فقط مقادیر عددی مجاز است ")]
            [MinLength(10)]
            public string PostalCode { get; set; }
            [MaxLength(20)]
            [MinLength(3)]
            [DisplayName(displayName: "شهر")]
            public string city { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var Address = user.Address;
            var PostalCode = user.PostalCode;
            var city = user.city;
            var FullName = user.FullName;
            var birthDate = user.brithDate;
            Input = new InputModel
            {
                Address = Address,
                PostalCode = PostalCode,
                city = city,
                PhoneNumber = phoneNumber,
                Username = userName,
                birthDate = birthDate,
                FullName = FullName,
            };
  
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //var TwostepAuthentication = await _userManager.GetTwoFactorEnabledAsync(user);
            //if (TwostepAuthentication)
            //{
            //    ViewData["TwoStepAuth"] = "Y";
            //    TwoStepAuth = "Y";
            //}
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if(Input.Username != User.Identity.Name)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user,Input.Username);
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage = "Error changing user name.";
                    return Page();
                }

            }
            if(Input.Address != user.Address)
            {
                user.Address = Input.Address;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    StatusMessage = "uexpected error to change address";
                    return Page();
                }
            }
            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    StatusMessage = "uexpected error to change postal code";
                    return Page();
                }
            }
            if (Input.city != user.city)
            {
                user.city = Input.city;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    StatusMessage = "uexpected error to change city";
                    return Page();
                }
            }
            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    StatusMessage = "uexpected error to change fullName";
                    return Page();
                }
            }
            if (Input.birthDate != user.brithDate && Input.birthDate < DateTime.Now)
            {
                user.brithDate = Input.birthDate;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    StatusMessage = "your age is under the legal age";
                    return Page();
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
