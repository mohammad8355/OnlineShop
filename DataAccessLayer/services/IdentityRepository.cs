using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.services
{
    public class IdentityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityRepository(UserManager<IdentityUser> _userManager)
        {
            this._userManager = _userManager;
        }
        public async Task<IdentityResult> RegisterAsync(IdentityUser identityUser)
        {
          var resualt =  await _userManager.CreateAsync(identityUser);
            return resualt;
        }
        //public async Task<IdentityResult> Login(IdentityUser identityUser)
        //{
    
        //    return resualt;
        //}
    }
}
