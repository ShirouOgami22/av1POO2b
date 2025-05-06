/*
!!!!!  sanitize database related input for ' (single quote), errors and sql injection danger!!
btw, change all the $"{}" for @ thingyies, related to database stuff so i can avoid sql injectionsssssssssss... not that i need to... its a school project
switch from local database to xampp localhost's
connect the code to the frontend with that [apicontroller] thingy...
get random books
*/
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.OleDb;


namespace LibraryApi.Controllers{
    [ApiController]
    [Route("library/query")]
    public class LibraryController : ControllerBase{
        
    }
}
class Program{
public static string current = "library";
public static bool running=true;
public static string worker="";
public static string permissions="";

    public static List<string> availableTables = new List<string>();
    public class Database{
    private MySqlConnection _db;
    public Database(){
        try{
            _db = new MySqlConnection("server=localhost;user=root;password=;database=library");

        }catch(Exception err){
            print($"No database found?\n{err}");
            Environment.Exit(1);
        }
    }
    public void connect(){
        try{
            if(_db.State!=System.Data.ConnectionState.Open){
                _db.Open();
                print("Connected to the database!");
            }else{print("Database already connected");}
        }catch(Exception){print($"Error connecting to database\nCheck if server is up\nThe program is set to connect to localhost database named 'library'");Environment.Exit(1);}
                using (var command = new MySqlCommand("select * from book", _db)){
                    using (var reader = command.ExecuteReader()){
                        while (reader.Read()){
                            availableTables.Add(reader.GetValue(0).ToString()!);
                        }
                    }    
                }
                List<Tuple<int,string,string>> books= new List<Tuple<int,string,string>>();
                using (var command = new MySqlCommand("select * from book", _db)){
                    using (var reader = command.ExecuteReader()){
                        while (reader.Read()){
                            books.Add(new Tuple<int,string,string>(
                                Convert.ToInt32(reader.GetValue(0)),
                                reader.GetValue(1).ToString()!,
                                reader.GetValue(2).ToString()!));
                        }
                    }    
                }

                //debug:
                    //list all books:
                    //for(int i=0;i<books.Count;i++){
                    //    print($"{books[i].Item1}, {books[i].Item2}, {books[i].Item3}");
                    //}
                    
                    //list all tables:
                    //for(int i=0;i<availableTables.Count;i++){
                    //    print($"{availableTables[i]}");
                    //}
                
    }
     public void disconnect(){
        try{
            if(_db.State!=System.Data.ConnectionState.Closed){
                _db.Close();
                print("Disconnected the database!");
            }else{print("No database connected");}
        }catch(Exception err){print($"Error disconnecting database:\n-'{err}'-");}
    }
    public bool checkTableCols(string columnName,string table="book"){
        using (var command = new MySqlCommand($"select column_name from information_schema.columns where table_name = '{table}' and column_name = '{columnName}'", _db))
        using (var reader = command.ExecuteReader()){
            while (reader.Read()){
                for(int i=0;i<reader.FieldCount;i++){
                string currentColumn = reader.GetValue(i).ToString()!;
                if (currentColumn.ToLower() == columnName.ToLower())
                    return true;
                }
            }
        }
        return false;
    }
    public bool logIn(string a){
        //basic filter against sql injection... not that i need, its a school project lol
        if(isNull(a)){
            print("invalid user?");
            return false;
        }else{
            for(int i=0;i<a.Length; i++){
                if(a[i].Equals('-')){
                    print("What you trynna do?");
                    return false;
                }
            }
        }
        using(var cmd = new MySqlCommand($"select * from employee where name = '{a}';",_db))
        using(var reader = cmd.ExecuteReader()){
            int found=reader.FieldCount;
            while (reader.Read()){
                for(int i=0;i<found; i++){
                    if(reader.GetName(i)=="name"){
                        worker=reader.GetValue(i).ToString()!;
                    }if(reader.GetName(i)=="role"){
                        permissions=reader.GetValue(i).ToString()!;
                    }
                    if((!isNull(worker))&&(!isNull(permissions))){
                       return true; 
                    }
                }
                return false;
            }
        }
        return false;
    }
    public bool paswd(string a,string b){

        using(var cmd = new MySqlCommand($"select pasword from employee where name = '{a}';",_db))
        using(var reader = cmd.ExecuteReader()){
            while(reader.Read()){
                string pass=reader.GetValue(0).ToString()!.ToLower();
                if(isNull(b)){
                    print("Type your password");
                    return false;
                }
                if(b==pass){
                    return true;
                }else{
                    print("Incorrect password");
                    return false;
                }
            }
        }
        return false;
    }
    public void querry(string method,object content){
        string sqlcomand;
        //print($"M:{method}, C:{content}"); //debugg...
        if(content.GetType() == typeof(int)){
            if(!checkTableCols(method)){
                print("Unknown method");
                return;
            }
            sqlcomand=$"select * from book where {method}={content}";
        }else if(content.GetType() == typeof(string)){
            sqlcomand=$"select * from book where {method}='{content}'";
        }else{
            print("Unknown type of content? nor text or number, is it decimal?");
            return;
        }
        using(var cmd = new MySqlCommand(sqlcomand,_db))
        using(var reader = cmd.ExecuteReader()){
            int counting=reader.FieldCount;
            while(reader.Read()){
                if(counting==0){
                    print("Book not found\n");
                    break;
                }
                print("");
                for (int e = 0; e < counting; e++){
                    print($"{reader.GetName(e)}: {reader.GetValue(e)}");
                }
                print("");
            }
        } 
    }   
    public void querry(string mode){
        string comando;
        if(mode=="all"){
            comando="select * from book";
        }else if(mode=="available"){
            comando="select * from book where available=1";
        }else if(mode=="unavailable"){
            comando="select * from book where available=0";
        }else{
            print("Unknown mode");
            return;
        }
        using(var cmd = new MySqlCommand(comando,_db))
            using(var reader = cmd.ExecuteReader()){
                int found=reader.FieldCount;
                while(reader.Read()){
                    if(found==0){
                        print("No books found");
                        return;
                    }
                    print("");
                    for(int e = 0; e < found; e++){
                        print($"{reader.GetName(e)}: {reader.GetValue(e)}");
                    }
                    print("");
                }
            }
    }
    public void create(string what){
    current = "creating";
    using (var cmd = new MySqlCommand($"SELECT table_name FROM information_schema.tables WHERE table_name = '{what}'", _db))
    using (var reader = cmd.ExecuteReader()){
        bool ok = false;
        if (!reader.HasRows){
            print("Cannot create item for unexisting table");
            return;
        }
        while (reader.Read()){
            for(int i = 0; i < reader.FieldCount; i++){
                if(reader.GetValue(i).ToString() == what){
                    ok = true;
                    break;
                }
            }
            if (ok) break;
        }
        if(!ok){
            print("Cannot create item for unexisting table");
            return;
        }
    }
    using (var cmd = new MySqlCommand($"SELECT column_name, extra FROM information_schema.columns WHERE table_name = '{what}' AND table_schema = DATABASE()", _db))
    using (var reader = cmd.ExecuteReader()) {
        List<string> cols = new List<string>();
        while (reader.Read()) {
            string extra = reader.IsDBNull(1) ? "" : reader.GetString(1);
            if (extra.ToLower().Contains("auto_increment")){
                continue;
            }

            cols.Add(reader.GetString(0));
        }
        if (cols.Count == 0){
            print("Something went wrong");
            return;
        }

        for (int i = 0; i < cols.Count; i++){
            while (true) {
                object? inp = string.Join(" ",input($"{current}: {what}: {cols.ElementAt(i)}> ")![0]);
                if (isNull(inp.ToString()!)) {
                    print("Cannot be empty!");
                    continue;
                }
                print($"{inp.GetType()}");
                break;
            }
        }

        // Still placeholder for actual insert
        // using(new MySqlCommand($"insert into {what} ",_db)){}
    }
}

    //insert into book(title,author,pubYear,category) VALUE ('Vovó Vigarista','David Williams',2013,'Fantasy');
    //UPDATE `book` SET `available` = '1' WHERE `book`.`id` = 95;
    //ALTER TABLE `book` CHANGE `available` `available` INT(11) NULL DEFAULT '1';
    //

}

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
    public static void print(string a){
        Console.WriteLine(a);
    }
    public static string[]? input(string a){
        Console.Write(a);
        string? txt = Console.ReadLine();;
        if(isNull(txt!)){
            return Array.Empty<string>();
        }else{
             return Regex.Matches(txt!, @"(?:\"".*?\"")|(?:\S+)").Cast<Match>().Select(m => m.Value.Trim('"')).ToArray();
        }
    }
    public static Database db = new Database();
    static void Main(string[] args){
        //Console.Clear();
        db.connect();
        if(args.Length==0){
            print("Please state whether you wish to manage the database with 'database' or start the program with 'library'\nYou may choose wheter to use a graphic user interface: 'noGUI' or 'GUI'");
            print("<program>.exe library nogui");
        }else{
            if(args[0].ToLower()=="library" && args[1].ToLower()=="nogui"){
                libraryNogui();
            }else if(args[0].ToLower()=="library" && args[1].ToLower()=="gui"){
                //
            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="nogui"){
                //attempt connection to an account in the database
                while(true){
                    string user=string.Join(" ",input("USER: ")!);
                    if(user.Length==0){
                        print("Type something");
                    }else{
                        if(db.logIn(user)){
                            break;
                        }
                        print($"No user found with the name {user}");
                    }
                }
                while(true){
                    string password=string.Join(" ",input("PSWD: ")!);
                    if(!isNull(worker)){
                        if(db.paswd(worker,password)){
                            print($"Logged in as {worker}!");
                            break;
                        }
                    }else{
                        print("user not selected\nTerminating program");
                        return;
                    }
                }
                if(permissions=="manager"){
                    databaseNogui();
                }else{
                    print($"You, {worker}, dont have direct acess to the database");
                }
            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="gui"){
                //
            }else{
                print("Unknown arguments\nexample of correct use:\n<program>.exe library nogui");
            }

        }
    }
    public static bool isNull(string a){if(a==null||a==""){return true;}return false;}
    static void select(){
        //Console.Clear();
        print(man("select","use"));
        print("You may <search> for a book while selecting\nType 'q' to cancel select");
        print(man("search","use"));
        bool selecting=true;
        while(selecting){
            current="selecting";
            string[]? a=input($"{current}> ");
            if(a?.Length==0){
                print("Please select a book by its ID");
            }else{
                switch(a?[0]){
                    case "":
                        print("Invalid?");
                    break;
                    case "q":
                    return;
                    case "h" or "help":
                    print("help?");//implement help...
                    break;
                    case "search":
                        if(a.Length>3 || a.Length<2){
                            print("Invalid amount of arguments");
                            search();
                        }else if(a.Length==2 && a[1]=="all"){
                            db.querry("all");
                        }else{
                            search(a[1],a[2]);
                        }
                    break;
                    case "select":
                        //selects/adds book to a list of selected books...
                    break;
                    case "remove":
                        //removes selected books
                    break;
                    case "list":
                        //lists selected books
                    break;
                    default:
                        print($"Unknown Command {a?[0]}");
                    break;
                } 
            }
        }
    }
static void search(string a = "default", object b = null!) {
    current = "searching";
    bool processed = false;

    if (a == "default" || b == null) {
        print("Type 'q' or 'quit' to cancel\nType 'h' or 'help'");
        print(man("search", "use"));
        return;
    }
    if (!processed) {
        if (a == "pubyear" || a == "id") {
            try { b = int.Parse(b!.ToString()!); } catch (Exception) {
                print($"For {a}, content must be numbers");
            }
        } else {
            b = b!.ToString()!;
        }
    }

    db.querry(a, b!);
}
    static void libraryNogui(){
        //Console.Clear();
        print("Type 'help' or 'h'");
        while(running){
            current = "library";
            string[]? a=input($"{current}> ");
            if(a!.Length==0){print("Type 'h' or 'help'");continue;}
                switch(a[0]){
                    case "" or null:
                        print("Please input something");
                    break;
                    //
                    case "h" or "help":
                    if(a.Length==1){
                        //could improve this tho...
                        print($"{man("clear","word")}\n{man("clear","desc")}\n{man("clear","use")}\n");
                        print($"{man("help","word")}\n{man("help","desc")}\n{man("help","use")}\n");
                        print($"{man("quit","word")}\n{man("quit","desc")}\n{man("quit","use")}\n");
                        print($"{man("search","word")}\n{man("search","desc")}\n{man("search","use")}\n");
                        print($"{man("select","word")}\n{man("select","desc")}\n{man("select","use")}\n");
                    }else if(a.Length==2){
                        if(commands.ContainsKey(a[1].ToLower())){
                            print($"{man(a[1],"word")}\n{man(a[1],"desc")}\n{man(a[1],"use")}\n");
                        }else{
                            print("Command doesnt exist");
                        }
                    }else{
                        print("Invalid amount of arguments");
                    }
                    //improve...
                    break;
                    //
                    case "q" or "quit":
                        db.disconnect();
                        print("Byye!");
                        Environment.Exit(0);
                    break;
                    //
                    case "cls" or "clear":
                        Console.Clear();
                    break;
                    //
                    case "search":
                    object e;
                    if(a.Length<2){
                        print("Invalid amount of arguments");
                        continue;
                    }else if(a.Length==2){
                        e=a[1];
                        a[1]="title";
                        print($"Searching by title: {e}");
                    }else{
                        if(!db.checkTableCols(a[1])){print("Table doesnt exist");continue;}
                        if(a[1].ToLower()=="pubyear"){a[1]="pubYear";}
                        if(a[1]=="pubYear"||a[1].ToLower()=="id"){
                            try{e=int.Parse(a[2]);}catch(Exception){
                                print($"For {a[1]}, content must be numbers");
                                continue;
                            }
                        }else{
                            e=string.Join(" ",a.Skip(2));
                        }
                    }
                        //print($"{a[0]}, {a[1]}, {e}, {m}");
                        search(a[1],e!);
                    break;
                    //
                    case "borrow":
                        //borrow();
                    break;
                    //
                    case "list":
                    if(a.Length==2){
                        if(isNull(a[1])){
                                print("Invalid argument to list");
                                break;
                        }else if(a[1]=="all"||a[1]=="available"||a[1]=="unavailable"){
                            db.querry(a[1]);
                        }else{
                            print("invalid filter");
                            break;
                        }
                    }else{
                        print("Invalid amount of arguments");
                    }
                    break;
                    //
                    case "select":
                        select();
                    break;
                    //
                    default:
                        print($"Unknown Command: '{a[0]}'");
                    break;
                }
        }
    }
    static void databaseNogui(){
        //Console.Clear();
        print("Type 'help' or 'h'");
        while(running){
            current = "database";
            string[]? a=input($"{current}> ");
            if(a?.Length>0){
                switch(a[0]){
                    case "" or null:
                    print("Please input something");
                break;
                    //
                    case "h" or "help":
                    if(a.Length==1){
                        //help menu for database commands...
                    }else if(a.Length==2){
                        //thats gonna be a pain in my ass...
                    }else{
                        print("Invalid amount of arguments");
                    }
                break;
                    //
                    case "q" or "quit":
                    print("Byye!");
                    db.disconnect();
                    Environment.Exit(0);
                break;
                    //
                    case "cls" or "clear":
                    Console.Clear();
                break;
                    //
                    case "query":
                        string method="";
                        string content="";
                        if(a.Length>1&&a.Length<5){
                            if(!isNull(a[1])){method=a[1];}
                            if(!isNull(a[2])){content=a[2];}
                        }else{
                            print("Incorrect amount of arguments");
                        }
                        while(true){
                            if(isNull(method)){
                                method=input("mode: ")![0];
                            }
                            if(isNull(content)){
                                content=string.Join(" ",input("content: ")!);
                            }
                            if(!isNull(method)&&!isNull(content)){break;}
                        }
                        db.querry(method,content);
                    break;
                    //
                    case "list":
                    
                    break;
                    //
                    case "create":
                        if(a.Length!=2){
                            print("Invalid amount of arguments");
                            break;
                        }
                        db.create(a[1]);
                    break;
                    //
                    case "drop":

                break;
                    //
                    case "input":

                break;
                    //
                    case "update":

                break;
                    //
                    default:
                    print($"Unknown Command: '{a[0]}'");
                break;
                }
            }else{
                print("Type 'h' or 'help'");
            }
        }
    }
}