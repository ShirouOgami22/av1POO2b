using static improving.Improveding;
using static Datas.Idk;
using Microsoft.AspNetCore.Mvc;
using Datas;

namespace Controller
{
    [ApiController]
    [Route("library")]
    public class Controls : ControllerBase{
        [HttpGet("all")]
        public IActionResult GetBook([FromQuery] string query){
            if (isNull(query)){
                return BadRequest("Invalid query");
            }
            object result = db.querryWeb("all");
            if (isNull(result!.ToString()!)){
                return NotFound("No data found");
            }
            return Ok(result);
        }
    }
}