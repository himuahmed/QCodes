using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.DbObjects;
using QCodes.Repository;

namespace QCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QTagController : ControllerBase
    {
        private readonly IQTagRepository _qTagRepository;

        public QTagController(IQTagRepository qTagRepository)
        {
            _qTagRepository = qTagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var qrTags = await _qTagRepository.GetAllTags();

            if (qrTags != null)
            {
                return Ok(qrTags);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(string id)
        {
            var qrTag = await _qTagRepository.GetTagById(id);

            if(qrTag != null)
            {
                return Ok(qrTag);
            }
            else
            {
                return Ok(null);
            }
        }
    }
}
