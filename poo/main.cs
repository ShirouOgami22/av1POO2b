using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
class Program{
public static string current = "library";
public static bool running=true;
public static List<string> availableTables = new List<string>();
public class Database{
    private SqliteConnection _db;
    public Database(string path="library.db"){
        if(File.Exists(path)){
            print($"Loading database: {path}");
            _db = new SqliteConnection($"Data Source={path}");
        }else{
            print("No database found");
            Environment.Exit(1);
        }
    }
    public void connect(){
        try{
            if(_db.State!=System.Data.ConnectionState.Open){
                _db.Open();
                print("Connected to the database!");
            }else{print("Database already connected");}
        }catch(Exception err){print($"Error connecting to database:\n-'{err}'-");}
                using (var command = new SqliteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'", _db)){
                    using (var reader = command.ExecuteReader()){
                        while (reader.Read()){
                            availableTables.Add(reader.GetString(0));
                        }
                    }    
                }
                List<Tuple<int,string,string>> books= new List<Tuple<int,string,string>>();
                using (var command = new SqliteCommand("SELECT * FROM book", _db)){
                    using (var reader = command.ExecuteReader()){
                        while (reader.Read()){
                            books.Add(new Tuple<int,string,string>(
                                Convert.ToInt32(reader.GetString(0)),
                                reader.GetString(1),
                                reader.GetString(2)));
                        }
                    }    
                }
                //for(int i=0;i<books.Count;i++){
                //    print($"{books[i].Item1}, {books[i].Item2}, {books[i].Item3}");
                //}
                //for(int i=0;i<availableTables.Count;i++){
                //    print($"{availableTables[i]}");
                //}
                
    }
    public bool checkTableCols(string columnName,string table="book"){
        using (var cmd = new SqliteCommand($"PRAGMA table_info({table});", _db))
        using (var reader = cmd.ExecuteReader()){
            while (reader.Read()){
                string currentColumn = reader.GetString(1);
                if (currentColumn == columnName)
                    return true;
            }
        }
        return false;
    }
    public void disconnect(){
        try{
            if(_db.State!=System.Data.ConnectionState.Closed){
                _db.Close();
                print("Disconnected the database!");
            }else{print("No database connected");}
        }catch(Exception err){print($"Error disconnecting database:\n-'{err}'-");}
    }
    public void querry(string mode){
        if(mode=="all"){
                    using(var cmd = new SqliteCommand($"SELECT * FROM book",_db))
                        using(var reader = cmd.ExecuteReader()){
                            int counting=reader.FieldCount;
                            while(reader.Read()){
                                if(counting==0){
                                    print("Book not found");
                                    break;
                                }
                                for(int e = 0; e < counting; e++){
                                    if(reader.GetName(e)=="title"){print($"{reader.GetValue(e)}");}
                                }
                            }
                        }
        }else{
            print("Unknown mode");
        }
    }
    public void querry(string mode,string method,string content){
        if(checkTableCols(method)==false){
            print("Unknown Filter");
            return;
        }
            if(mode=="search"){
                using(var cmd = new SqliteCommand($"SELECT * FROM book WHERE {method}=\"{content}\"",_db))
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
        
    }
    public void querry(string mode,string method,int content){
        if(checkTableCols(method)==false){
            print("Unknown Filter");
            return;
        }
            if(mode=="search"){
                using(var cmd = new SqliteCommand($"SELECT * FROM book WHERE {method}={content}",_db))
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
        
    }
   
}
public static Dictionary<string,object> commands = new Dictionary<string,object>{
    //{"",new {word="",desc="",use=""}}
    {"clear",new {word="clear/cls",desc="Clears the terminal",use="'clear' or 'cls'"}},
    {"help",new {word="help/h",desc="Shows the command help or the use of a command",use="'help' <command> or 'h' <command>"}},
    {"quit",new {word="quit/q",desc="Terminates the program",use="'quit' or 'q'"}},
    {"search",new {word="search",desc="Searches for a specified book",use="search <method> <content>\nMethods: 'author', 'title', 'pubYear' or 'id'"}},
};

    public static string man(string com, string inf){
        if(commands.ContainsKey(com.ToLower())){
                var command = (dynamic)commands[com];
                return inf.ToLower() switch{
                    "word" => command.word,
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
        if(txt == null || txt==""){
            return Array.Empty<string>();
        }else{
             return Regex.Matches(txt, @"(?:\"".*?\"")|(?:\S+)").Cast<Match>().Select(m => m.Value.Trim('"')).ToArray();
        }

    }
    
    public static Database db = new Database("library.db");
    static void Main(string[] args){
        //Console.Clear();
        if(args.Length==0){
            print("Please state whether you wish to manage the database with 'database' or start the program with 'library'\nYou may choose wheter to use a graphic user interface: 'noGUI' or 'GUI'");
            print("<program>.exe library nogui");
        }else{
            if(args[0].ToLower()=="library" && args[1].ToLower()=="nogui"){
                libraryNogui();
            }else if(args[0].ToLower()=="library" && args[1].ToLower()=="gui"){

            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="nogui"){

            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="gui"){

            }else{
                print("Unknown arguments\nexample of correct use:\n<program>.exe library nogui");
            }

        }
    }

    static void select(){
        print("Select a book by its id");
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
                    case "search":
                        search();
                    break;
                    case "select":
                        
                    break;
                    default:
                        print($"Unknown Command {a?[0]}");
                    break;
                } 
            }
        }
    }
    static void search(string a="default", string b="default"){
        current = "searching";
        if(a=="all"){
            db.querry("all");
        }else if(a=="default"||b=="default"){
            bool searching=true;
            print("Type 'q' or 'quit' to cancel search");
            print(man("search","use"));
            while(searching){
                string[]? txt=input($"{current}> ");
                if(txt![0]=="q"||txt[0]=="quit"){
                    print("Exited searching");
                    return;
                }else if(txt?.Length<2 || txt![0]==null){
                    print("Invalid amount of arguments");
                }else{
                    a=txt![0];
                    b=txt![1];
                    searching=false;
                }
            }
        }
        db.querry("search",$"{a}",$"{b}");
    }
    public static void checkLibrary(){
        print("The library currently has:");
        print($"{"placeholder"} books\n{"placeholder"} categories");
    }
    static void libraryNogui(){
        //Console.Clear();
        db.connect();
        print("Type 'help' or 'h'");
        while(running){
            current = "library";
            string[]? a=input($"{current}> ");
            if(a?.Length>0){
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
                    print("Byye!");
                    Environment.Exit(0);
                break;
                //
                case "cls" or "clear":
                    Console.Clear();
                break;
                //
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
                //
                case "borrow":
                //borrow();
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
            }else{
                print("Type 'h' or 'help'");
            }
        }
    }
}