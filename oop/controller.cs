using System.Text.RegularExpressions;
using static improving.Improveding;
using Microsoft.AspNetCore.Mvc;
using static vars.Thingyes;
using System.Text.Json;
using static Datas.Idk;
using Datas;
namespace Controller{
    [ApiController]
    [Route("library")]
    public class Controls : ControllerBase{
        [HttpGet("rmBook")]
        public IActionResult delete([FromQuery] string query){
            if(permissions=="manager"){
                db.remove(Convert.ToInt32(query));
                return Ok($"Deleted book ${query}");
            }
            return BadRequest("You dont have the right permission");
        }

        [HttpGet("rmUser")]
        public IActionResult deleteUser([FromQuery] string query){
            if(permissions=="manager"){
                db.removeUser(Convert.ToInt32(query));
                return Ok($"Deleted user ${query}");
            }
            return BadRequest("You dont have the right permission");
        }

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
        public IActionResult getAll([FromQuery] string method,[FromQuery] string order){
            return Ok(db.querry("all",method,order));
        }
        
        [HttpGet("GetUsers/all")]
        public IActionResult getAusers(){
            if(permissions!="manager"){
                return BadRequest("You dont have the permission");
            }
            return Ok(db.querryUsers("all"));
        }
        
        [HttpGet("GetBook/{aspect}")]
        public IActionResult getBook(string aspect, [FromQuery] string query,[FromQuery] string? method,[FromQuery] string? order){
            if(method==null||method==""){method="id";}
            if(order==null||order==""){order="asc";}
            return Ok(db.querry(aspect,query,method,order));
        }

        [HttpPost("Create/{what}")]
        public IActionResult create(string what,[FromBody] JsonElement body){
            if(permissions=="manager"){
                if(what == "book"){
                    var title = body.GetProperty("title").GetString();
                    var author = body.GetProperty("author").GetString();
                    var pubyear = body.GetProperty("pubYear").GetInt32();
                    var category = body.GetProperty("category").GetString();
                    db.createBook(title!,author!,pubyear!,category!);
                }else if(what=="user"){
                    var name = body.GetProperty("name").GetString();
                    var role = body.GetProperty("role").GetString();
                    db.createUser(name!,role!);
                }
                return Ok();
            }else{
                return BadRequest("Youre not a manager...");
            }
        }
        [HttpPost("Edit")]
        public IActionResult editBook([FromBody] JsonElement body){
                var id= body.GetProperty("id").GetInt32();
                var title= body.GetProperty("title").GetString();
                var author= body.GetProperty("author").GetString();
                var pubyear= body.GetProperty("pubYear").GetInt32();
                var category= body.GetProperty("category").GetString();
            db.editBook(id!,title!,author!,pubyear!,category!);
            return Ok();
        }
        [HttpGet("LogIn")]
        public IActionResult getUser([FromQuery] string query){
            string[] querries=query.Split(";");
            if(querries.Length>2||querries.Length<2){return BadRequest("Invalid query");}
                string user=querries[0];string password=querries[1];
                if(!(db.logIn(user)&&db.paswd(password,user))){
                    return BadRequest("Invalid Name or password");
                }
                return Redirect("/main/index.html");
        }
    }   
}