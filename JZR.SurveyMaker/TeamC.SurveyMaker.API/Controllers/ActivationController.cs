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
    public class ActivationController : ControllerBase
    {
        // GET: api/<ActivationController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activation>>> Get()
        {
            try
            {
                return Ok(await ActivationManager.Load());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST api/<ActivationController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Activation activation, bool rollback = false)
        {
            try
            {
                return Ok(await ActivationManager.Insert(activation, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT api/<ActivationController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Activation activation, bool rollback = false)
        {
            try
            {
                return Ok(await ActivationManager.Update(activation, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, bool rollback = false)
        {
            try
            {
                return Ok(await ActivationManager.Delete(id, rollback));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
