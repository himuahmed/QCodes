using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QCodes.Models;
using QCodes.Repository;

namespace QCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAndPersonController : ControllerBase
    {
        private readonly IUserAndPersonRepository _userAndPersonRepository;

        public UserAndPersonController(IUserAndPersonRepository userAndPersonRepository)
        {
            _userAndPersonRepository = userAndPersonRepository;
        }

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

        public async Task<IActionResult> GetPersonByUserId(string userId)
        {
            var person = await _userAndPersonRepository.GetPersonByUserId(userId);

            if (person != null)
            {
                return Ok(person);
            }
            else
            {
                return BadRequest("Unable to get the person.");
            }
        }
    }
}
