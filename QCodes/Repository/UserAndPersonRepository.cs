using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.DbObjects;
using QCodes.Models;

namespace QCodes.Repository
{
    public class UserAndPersonRepository : IUserAndPersonRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserAndPersonRepository(DataContext dataContext, IMapper Mapper)
        {
            _dataContext = dataContext;
            _mapper = Mapper;
        }

        public async Task<PersonModel> AddPersonDetails(PersonModel personModel)
        {
            var personToBeInserted = _mapper.Map<Person>(personModel);
            var returnedPerson = await _dataContext.AddAsync(personToBeInserted);
            await _dataContext.SaveChangesAsync();

            return _mapper.Map<PersonModel>(returnedPerson);
        }

        public async Task<PersonModel> GetPersonByUserId(string userId)
        {
            var returnedPerson = await _dataContext.Persons.FirstOrDefaultAsync(P => P.UserId == userId);
            return _mapper.Map<PersonModel>(returnedPerson);
        }


    }
}
