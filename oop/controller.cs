using static improving.Improveding;
using static Datas.Idk;
using Microsoft.AspNetCore.Mvc;
using Datas;
namespace Controller{
    [ApiController]
    [Route("library")]
    public class Controls : ControllerBase{
        [HttpGet("Count")]
        public IActionResult Tbooks(){
            print(db.count.ToString());
            return Ok(db.count.ToString());
        }
        [HttpGet("ListBooks/all")]
        public IActionResult getAll(){
            return Ok(db.querry("all"));
        }

        [HttpGet("GetBook/{aspect}")]
        public IActionResult getBook(string aspect, [FromQuery] string query){
            return Ok(db.querry(aspect,query));
        }
    }   
}