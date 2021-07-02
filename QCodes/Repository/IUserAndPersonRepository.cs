using System.Threading.Tasks;
using QCodes.Models;

namespace QCodes.Repository
{
    public interface IUserAndPersonRepository
    {
        Task<PersonModel> AddPersonDetails(PersonModel personModel);
        Task<PersonModel> GetPersonByUserId(string userId);
    }
}