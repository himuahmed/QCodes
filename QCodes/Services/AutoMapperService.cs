using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QCodes.DbObjects;
using QCodes.Models;

namespace QCodes.Services
{
    public class AutoMapperService : Profile
    {
        public AutoMapperService()
        {
            CreateMap<Person, PersonModel>();
            CreateMap<PersonModel, Person>();
            CreateMap<GlobalMessageModel, GlobalMessage>();
            CreateMap<GlobalMessage, GlobalMessageModel>();
            CreateMap<BloodRequestModel, BloodRequest>();
            CreateMap<BloodRequest, BloodRequestModel>();
            //CreateMap<PaginationService<Person>, PersonModel>();
            //CreateMap<PaginationService<PersonModel>, Person>();

        }
    }
}
