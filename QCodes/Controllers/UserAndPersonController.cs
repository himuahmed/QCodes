using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QCodes.Models;
using QCodes.Repository;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QCodes.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace QCodes.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAndPersonController : ControllerBase
    {
        private readonly IUserAndPersonRepository _userAndPersonRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAndPersonController(IUserAndPersonRepository userAndPersonRepository, IMapper mapper, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _userAndPersonRepository = userAndPersonRepository;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
        }
        
        
        protected string GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value; 
            return userId;
        }

        protected string GetUserEmail()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return userEmail;
        }

        [HttpGet("getLoggedInUser")]
        public async Task<IActionResult> getLoggedInUser()
        {
            var userId = GetUserId();
            var returnedPerson = await _userAndPersonRepository.GetPersonByUserId(userId);
            var person = _mapper.Map<PersonModel>(returnedPerson);
            return Ok(person);
          
        }

        [HttpPost("addPersonData")]
        public async Task<IActionResult> AddPersonDate(PersonModel personModel)
        {
            personModel.UserId = GetUserId();
            personModel.Email = GetUserEmail();
            personModel.Country = "Bangladesh";
            personModel.PublicProfile = true;
            personModel.CreatedAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(personModel);
            }

            var person = await _userAndPersonRepository.AddPersonDetails(personModel);
            if (person != null)
            {
                return Ok(person);
            }
            return BadRequest("Unable to add person details for " + personModel.FullName);
        }

        [HttpGet("personByUserId/{userId}")]
        public async Task<IActionResult> GetPersonByUserId(string userId)
        {
            var returnedPerson = await _userAndPersonRepository.GetPersonByUserId(userId);
            var person = _mapper.Map<PersonModel>(returnedPerson);

            if (person != null)
            {
                return Ok(person);
            }
            else
            {
                return BadRequest("Unable to get the person.");
            }
        }


        [HttpPut("updatePerson")]
        public async Task<IActionResult> UpdatePersonData(PersonModel personModel)
        {
            if (!ModelState.IsValid) return BadRequest();

           // if (await GetPersonByUserId(personModel.UserId) == null) return BadRequest();

            var person = await _userAndPersonRepository.UpdatePersonDetail(personModel);
            if (person)
            {
                return Ok(personModel);
            }

            return BadRequest();
        }

        [HttpGet("getAllDonors")]
        public async Task<IActionResult> GetAllDonors([FromQuery] UserParams userParams)
        {
            var personList = await _userAndPersonRepository.GetAllBloodDonors( userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }


        [HttpGet("dis/{district}")]
        public async Task<IActionResult> GetPersonByDistrict(string district, [FromQuery] UserParams userParams)
        {
            if (district == null) return BadRequest();

            var personList = await _userAndPersonRepository.GetPersonByDistrict(district, userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("dis/{district}/{bloodGroup}")]
        public async Task<IActionResult> GetPersonByDistrictAndBloodGroup(string district,string bloodGroup, [FromQuery] UserParams userParams)
        {
            if (district == null) return BadRequest();
            if (bloodGroup == null) return BadRequest();
            if (bloodGroup.Length > 8)
            {
                bloodGroup = bloodGroup.Substring(0, bloodGroup.Length - 8) + "+";
            }

            var personList = await _userAndPersonRepository.GetPersonByDistrictAndBloodGroup(district, bloodGroup,userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("div/{division}")]
        public async Task<IActionResult> GetPersonByDivision(string division, [FromQuery] UserParams userParams)
        {
            if (division == null) return BadRequest();

            var personList = await _userAndPersonRepository.GetPersonByDivision(division, userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("div/{division}/{bloodGroup}")]
        public async Task<IActionResult> GetPersonByDivisionAndBloodGroup(string division, string bloodGroup, [FromQuery] UserParams userParams)
        {
            if (division == null) return BadRequest();
            if (bloodGroup == null) return BadRequest();
            if (bloodGroup.Length > 8)
            {
                bloodGroup = bloodGroup.Substring(0, bloodGroup.Length - 8) + "+";
            }

            var personList = await _userAndPersonRepository.GetPersonByDivisionAndBloodGroup(division, bloodGroup, userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("uni/{union}")]
        public async Task<IActionResult> GetPersonByUnion(string union, [FromQuery] UserParams userParams)
        {
            if (union == null) return BadRequest();

            var personList = await _userAndPersonRepository.GetPersonByUnion(union, userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("uni/{union}/{bloodGroup}")]
        public async Task<IActionResult> GetPersonByUnionAndBloodGroup(string union, string bloodGroup, [FromQuery] UserParams userParams)
        {
            if (union == null) return BadRequest();
            if (bloodGroup == null) return BadRequest();
            if (bloodGroup.Length > 8)
            {
                bloodGroup = bloodGroup.Substring(0, bloodGroup.Length - 8) + "+";
            }

            var personList = await _userAndPersonRepository.GetPersonByUnionAndBloodGroup(union, bloodGroup, userParams);
            foreach (var person in personList)
            {
                if (person.ContactNoVisible == false)
                {
                    person.ContactNo = "N/A";
                }

                if (person.EmailVisible == false)
                {
                    person.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(personList);
        }

        [HttpGet("b/{group}")]
        public async Task<IActionResult> GetPersonByBloodGroup(string group, [FromQuery] UserParams userParams)
        {
            if (group == null) return BadRequest();
            if(group.Length > 8)
            {
                group = group.Substring(0, group.Length - 8) + "+";
            }

            var person = await _userAndPersonRepository.GetPersonByBloodGroup(group, userParams);

            Response.Headers(person.CurrentPage, person.PageSize, person.TotalCount, person.TotalPage);
            foreach (var per in person)
            {
                if (per.ContactNoVisible == false)
                {
                    per.ContactNo = "N/A";
                }

                if (per.EmailVisible == false)
                {
                    per.Email = "N/A";
                }
            }
            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
        }

    }
}
