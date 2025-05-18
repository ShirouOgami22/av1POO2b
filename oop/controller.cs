using System.Text.RegularExpressions;
using static improving.Improveding;
using Microsoft.AspNetCore.Mvc;
using static vars.Thingyes;
using static Datas.Idk;
using Datas;
namespace Controller{
    [ApiController]
    [Route("library")]
    public class Controls : ControllerBase{
        [HttpGet("UserCreds")]
        public IActionResult auth(){
            if(permissions=="manager"){
                return Ok("manager");
            }else{
                return Ok("user");
            }
        }
        [HttpGet("Count")]
        public IActionResult Tbooks(){
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
        [HttpGet("LogIn")]
        public IActionResult getUser([FromQuery] string query){
            string[] querries= query.Split(";");
            if(querries.Length>2||querries.Length<2){return BadRequest("Invalid query");}
                string user=querries[0];string password=querries[1];
                if(!(db.logIn(user)&&db.paswd(password,user))){
                    return BadRequest("Invalid Name or password");
                }
                return Redirect("/index.html");
        }
    }   
}