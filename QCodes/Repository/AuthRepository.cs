using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.Models;

namespace QCodes.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> UserExists(string userName)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == userName))
                return true;

            return false;
        }


        public async Task<IdentityResult> UserRegister(UserRegistrationModel userRegistrationModel)
        {
            var user = new IdentityUser()
            {
                Email = userRegistrationModel.Email,
                UserName = userRegistrationModel.Email,
            };

            return await _userManager.CreateAsync(user, userRegistrationModel.Password);
        }

        public async Task<IdentityUser> Login(LoginModel loginModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginModel.EmailOrUserName) ?? await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginModel.EmailOrUserName);

            if (user == null)
            {
                return null;
            }
            var result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!result)
            {
                return null;
            }
            return user;
        }


    }
}
