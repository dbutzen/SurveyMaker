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
    public class ResponseController : ControllerBase
    {

        // POST api/<QuestionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Response response)
        {
            try
            {
                return Ok(await ResponseManager.Insert(response));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
