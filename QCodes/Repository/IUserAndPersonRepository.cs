using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QCodes.Models;
using QCodes.DbObjects;
using System.Collections.Generic;
using QCodes.Services;

namespace QCodes.Repository
{
    public interface IUserAndPersonRepository
    {
        Task<PersonModel> AddPersonDetails(PersonModel personModel);
        Task<Person> GetPersonByUserId(string userId);
        Task<bool> UpdatePersonDetail(PersonModel personModel);
        Task<PaginationService<Person>> GetPersonByDistrict(string districtName, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByDistrictAndBloodGroup(string districtName, string bloodGroup, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByDivision(string divisionName, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByDivisionAndBloodGroup(string divisionName, string bloodGroup, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByUnion(string unionName, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByUnionAndBloodGroup(string unionName, string bloodGroup, UserParams userParams);
        Task<PaginationService<Person>> GetPersonByBloodGroup(string bloodGroup, UserParams userParams);
    }
}