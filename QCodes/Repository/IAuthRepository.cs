using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QCodes.Models;

namespace QCodes.Repository
{
    public interface IAuthRepository
    {
        Task<IdentityResult> UserRegister(UserRegistrationModel userRegistrationModel); 
        Task<bool> UserExists(string userName);
        Task<IdentityUser> Login(LoginModel loginModel);
    }
}