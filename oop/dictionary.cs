using static improving.Improveding;
using static vars.Thingyes;
using System.Text.RegularExpressions;
namespace extras{
    public static class extraFucns{
        public static Dictionary<string,object> commands = new Dictionary<string,object>{
    //{"",new {word="",desc="",use=""}}
    {"clear",new {word="clear/cls",desc="Clears the terminal",use="'clear' or 'cls'"}},
    {"help",new {word="help/h",desc="Shows the command list or the use of a command",use="'help' <command> or just 'h'"}},
    {"quit",new {word="quit/q",desc="Terminates the program",use="'quit' or 'q'"}},
    {"search",new {word="search",desc="Searches for a specified book",use="search <method> <content>\nMethods: 'author', 'title', 'pubYear' or 'id'"}},
    {"select",new {word="select",desc="Selects a specified book",use="select <book ID>"}},
    {"list",new {word="list",desc="Lists books",use="list <options>\nOptions: 'all', 'available','unavailable'"}},
};
        public static string man(string com, string inf){
        if(commands.ContainsKey(com.ToLower())){
                var command = (dynamic)commands[com];
                return inf.ToLower() switch{
                    "word" => $"'{command.word}'",
                    "desc" => command.desc,
                    "use" => command.use,
                    _ => "No such info"
                };
        }else{
            return "Command not found";
        }
    }

    }
}