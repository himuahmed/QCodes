using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QCodes.DbObjects;
using QCodes.Models;
using QCodes.Repository;
using QCodes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QCodes.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class BloodRequestController : ControllerBase
    {
        private readonly IBloodRequestRepository _bloodRequestRepository;
        private readonly IUserAndPersonRepository _userAndPersonRepository;
        private readonly IMapper _mapper;
        public BloodRequestController(IBloodRequestRepository bloodRequestRepository,IUserAndPersonRepository userAndPersonRepository, IMapper mapper)
        {
            _bloodRequestRepository = bloodRequestRepository;
            _userAndPersonRepository = userAndPersonRepository;
            _mapper = mapper;
        }

        [Route("requestBlood")]
        [HttpPost]
        public async Task<IActionResult> requestBlood(BloodRequestModel bloodRequestModel)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            bloodRequestModel.createdAt = DateTime.Now;
            bloodRequestModel.userId = userId;
            var mappedObj = _mapper.Map<BloodRequest>(bloodRequestModel);
            var res = await _bloodRequestRepository.RequestBlood(mappedObj);
            if(res != null)
            {
                return Ok(_mapper.Map<BloodRequestModel>(res));
            }
            return BadRequest("Try again later. Unable to request blood right now.");
            
        }


        [Route("getbloodRequestList")]
        [HttpGet]
        public async Task<IActionResult> blodReqList([FromQuery] BloodRequestsParams bloodRequestsParams)
        {
            var res =  await _bloodRequestRepository.GetAllBloodRequests(bloodRequestsParams);
            return Ok(res);
        }





    }
}
