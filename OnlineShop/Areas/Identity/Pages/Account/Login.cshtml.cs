// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Electrical.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _config;

        public LoginModel(IConfiguration config,SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
            _userManager = userManager;
        }

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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            // /// <summary>
            // ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            // ///     directly from your code. This API may change or be removed in future releases.
            // /// </summary>
            // [Required]
            // [MaxLength(100)]
            // [DataType(DataType.Text)]
            // [Display(Name = "نام کاربری")]
            // public string Name { get; set; }
            //
            // /// <summary>
            // ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            // ///     directly from your code. This API may change or be removed in future releases.
            // /// </summary>
            // [Required]
            // [DataType(DataType.Password)]
            // public string Password { get; set; }
            //
            // /// <summary>
            // ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            // ///     directly from your code. This API may change or be removed in future releases.
            // /// </summary>
            // [Display(Name = "Remember me?")]
            // public bool RememberMe { get; set; }
            public string PhoneNumber { get; set; }


            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null,CancellationToken cancellationToken = default)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.Users.Where(c => c.PhoneNumber == Input.PhoneNumber)
                    .FirstOrDefaultAsync(cancellationToken);
                if (user != null)
                {
                    var passwordcheck = await _userManager.CheckPasswordAsync(user,Input.Password);
                    if (!passwordcheck)
                    {
                        ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
                        return Page();
                    }
                    var token = GenerateJwtToken(user.Id,user.PhoneNumber);

                    // Store token in cookie (or localStorage in SPA scenario)
                    Response.Cookies.Append("jwt", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true
                    });
                        if (user.IsEnable)
                        {
                            var claims = new List<Claim>
                        {
                             new Claim(ClaimTypes.NameIdentifier,user.Id),
                             new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                             new Claim(ClaimTypes.Name,user.UserName),
                             new Claim(ClaimTypes.Role, "Administrator"),
                        };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = true,
                            };
                            await HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);

                        }else  if (user.TwoFactorEnabled)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
                    }
                   else if (user.LockoutEnabled && user.LockoutEnd.HasValue)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "خطایی رخ داده است ");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "خطایی رخ داده است ");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private string GenerateJwtToken(string Id,string PhoneNumber)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,Id.ToString()),
                new Claim("phone",PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
