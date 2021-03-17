using JZR.SurveyMaker.BL;
using JZR.SurveyMaker.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeamC.SurveyMaker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        // GET: api/<QuestionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> Get()
        {
            try
            {
                return Ok(await QuestionManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<QuestionController>/5
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Question>> Get(Guid id)
        {
            try
            {
                return Ok(await QuestionManager.LoadById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<QuestionController>/5
        [HttpGet("{activationCode}")]
        public async Task<ActionResult<Question>> LoadByActivationCode(string activationCode)
        {
            try
            {
                return Ok(await QuestionManager.LoadByActivationCode(activationCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<QuestionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Question question, bool rollback = false)
        {
            try
            {
                return Ok(await QuestionManager.Insert(question, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Question question, bool rollback = false)
        {
            try
            {
                return Ok(await QuestionManager.Update(question, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
