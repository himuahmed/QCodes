﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.DbObjects;
using QCodes.Models;
using QCodes.Services;

namespace QCodes.Repository
{
    public class UserAndPersonRepository : IUserAndPersonRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserAndPersonRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<Person> GetPersonByUserId(string userId)
        {
            var returnedPerson = await _dataContext.Persons.AsNoTracking().FirstOrDefaultAsync(P => P.UserId == userId);
            return returnedPerson;
        }

        public async Task<PersonModel> AddPersonDetails(PersonModel personModel)
        {
             var personToBeInserted = _mapper.Map<Person>(personModel);
             var returnedPerson = await _dataContext.AddAsync(personToBeInserted);
             var person = returnedPerson.Entity;
             await _dataContext.SaveChangesAsync();

             return _mapper.Map<PersonModel>(person);
        }

        public async Task<bool> UpdatePersonDetail(PersonModel personModel)
        {
            if(await GetPersonByUserId(personModel.UserId) == null) return false;
            var personToBeUpdated = _mapper.Map<Person>(personModel); 
            _dataContext.Persons.Update(personToBeUpdated);
            return await _dataContext.SaveChangesAsync() > 0 ? true : false;
        }

        //public async Task<bool> updateProfilePrivacy(string userId, bool isPublic)
        //{
        //    var person = await GetPersonByUserId(userId);
        //    if (person != null)
        //    {
        //        person.PublicProfile = isPublic;
        //    }

        //    _dataContext.Persons.Update(person);
        //    return await _dataContext.SaveChangesAsync() > 0 ? true : false;
        //}

        public async Task<PaginationService<Person>> GetPersonByDistrict(string districtName, UserParams userParams)
        {
            var person = _dataContext.Persons.Where(p => p.District == districtName && p.PublicProfile == true).AsQueryable();
           

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<PaginationService<Person>> GetPersonByDistrictAndBloodGroup(string districtName,string bloodGroup, UserParams userParams)
        {
            var person =  _dataContext.Persons.Where(p => p.District == districtName && p.BloodGroup == bloodGroup && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PaginationService<Person>> GetPersonByDivision(string divisionName, UserParams userParams)
        {
            var person =  _dataContext.Persons.Where(p => p.Division == divisionName && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PaginationService<Person>> GetPersonByDivisionAndBloodGroup(string divisionName, string bloodGroup, UserParams userParams)
        {
            var person = _dataContext.Persons.Where(p => p.Division == divisionName && p.BloodGroup == bloodGroup && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PaginationService<Person>> GetPersonByUnion(string unionName, UserParams userParams)
        {
            var person =  _dataContext.Persons.Where(p => p.Union == unionName && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PaginationService<Person>> GetPersonByUnionAndBloodGroup(string unionName, string bloodGroup, UserParams userParams)
        {
            var person = _dataContext.Persons.Where(p => p.Union == unionName && p.BloodGroup == bloodGroup && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PaginationService<Person>> GetPersonByBloodGroup(string bloodGroup, UserParams userParams)
        {
            var person =  _dataContext.Persons.Where(p => p.BloodGroup == bloodGroup && p.PublicProfile == true).AsQueryable();

            return await PaginationService<Person>.CreateAsync(person, userParams.PageNumber, userParams.PageSize);
            //return pagedPerson;

        }


    }
}
