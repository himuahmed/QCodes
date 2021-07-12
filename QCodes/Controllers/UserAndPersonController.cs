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
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using QCodes.Services;

namespace QCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAndPersonController : ControllerBase
    {
        private readonly IUserAndPersonRepository _userAndPersonRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserAndPersonController(IUserAndPersonRepository userAndPersonRepository,IMapper mapper,IConfiguration configuration)
        {
            _userAndPersonRepository = userAndPersonRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("addPersonData")]
        public async Task<IActionResult> AddPersonDate(PersonModel personModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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


        [HttpGet("dis/{district}")]
        public async Task<IActionResult> GetPersonByDistrict(string district, [FromQuery] UserParams userParams)
        {
            if (district == null) return BadRequest();

            var person = await _userAndPersonRepository.GetPersonByDistrict(district, userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
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

            var person = await _userAndPersonRepository.GetPersonByDistrictAndBloodGroup(district, bloodGroup,userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
        }

        [HttpGet("div/{division}")]
        public async Task<IActionResult> GetPersonByDivision(string division, [FromQuery] UserParams userParams)
        {
            if (division == null) return BadRequest();

            var person = await _userAndPersonRepository.GetPersonByDivision(division, userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
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

            var person = await _userAndPersonRepository.GetPersonByDivisionAndBloodGroup(division, bloodGroup, userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
        }

        [HttpGet("uni/{union}")]
        public async Task<IActionResult> GetPersonByUnion(string union, [FromQuery] UserParams userParams)
        {
            if (union == null) return BadRequest();

            var person = await _userAndPersonRepository.GetPersonByUnion(union, userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
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

            var person = await _userAndPersonRepository.GetPersonByUnionAndBloodGroup(union, bloodGroup, userParams);

            //if (!person.Any()) return Ok("No resord.");

            return Ok(person);
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

           // if (!person.Any()) return Ok("No resord.");

            return Ok(person);
        }

    }
}
