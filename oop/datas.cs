using static improving.Improveding;

using static vars.Thingyes;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Datas{
    public class Database(){
        static private MySqlConnection? _db=new MySqlConnection("Server=localhost;Database=library;User=root;Password=;");
        public int count=0;
        public void connect(){
        try{
            if(_db!.State!=System.Data.ConnectionState.Open){
                _db.Open();
                print("Connected to the database!");
            }else{print("Database already connected");}
        }catch(Exception){
            print($"Error connecting to database\nCheck if server is up\nThe program is set to connect to localhost database named 'library'");
            Environment.Exit(1);
        }
        using(var cmd = new MySqlCommand("select count(*) from book",_db)){
            count=Convert.ToInt32(cmd.ExecuteScalar());
        }
        using (var cmd = new MySqlCommand("select * from book", _db)){
            using (var reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    availableTables.Add(reader.GetValue(0).ToString()!);
                }
            }    
        }
        List<Tuple<int,string,string>> books= new List<Tuple<int,string,string>>();
        using (var cmd = new MySqlCommand("select * from book", _db)){
            using (var reader = cmd.ExecuteReader()){
                
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
            if(_db!.State!=System.Data.ConnectionState.Closed){
                _db.Close();
                print("Disconnected the database!");
            }else{print("No database connected");}
        }catch(Exception err){print($"Error disconnecting database:\n-'{err}'-");}
    }

        public bool checkTableCols(string columnName,string table=@"book"){
        using (var command = new MySqlCommand($"select column_name from information_schema.columns where table_name = @table and column_name = @columnName", _db)){
        command.Parameters.AddWithValue("@table", table);
        command.Parameters.AddWithValue("@columnName", columnName);
        using (var reader = command.ExecuteReader()){
            while (reader.Read()){
                for(int i=0;i<reader.FieldCount;i++){
                string currentColumn = reader.GetValue(i).ToString()!;
                if (currentColumn.ToLower() == columnName.ToLower())
                    return true;
                }
            }
        }}
        return false;
    }

        public bool logIn(string a){
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
        using(var cmd = new MySqlCommand($"select * from user where name = @a;",_db)){
        cmd.Parameters.AddWithValue("@a", a);
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
        }}
        return false;
    }

        public bool paswd(string b,string a){
            if(a!=worker){return false;}
            using(var cmd = new MySqlCommand($"select pasword from user where name = @a;",_db)){
            cmd.Parameters.AddWithValue("@a",a);
            using(var reader = cmd.ExecuteReader()){
                while(reader.Read()){
                    string pass=reader.GetValue(0).ToString()!;
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
            }}
            return false;
        }

        public object querry(string mode){
        string comando;
        if(mode=="all"){
            comando="select * from book";
        }else if(mode=="available"){
            comando="select * from book where available=1";
        }else if(mode=="unavailable"){
            comando="select * from book where available=0";
        }else{
            print("Unknown mode");
            return null!;
        }
        List<Dictionary<string,object>> results = new List<Dictionary<string,object>>();
        using(var cmd = new MySqlCommand(comando,_db))
            using(var reader = cmd.ExecuteReader()){
                int count=reader.FieldCount;
                if(!reader.HasRows){return null!;}
                while(reader.Read()){
                    var row = new Dictionary<string,object>();
                    for (int e = 0; e < count; e++){
                        row[reader.GetName(e)] = reader.GetValue(e);
                    }
                    results.Add(row);
                }
            }
        return results;
    }

        public object querry(string method,object content){
        if(!checkTableCols(method)){
            print("Unknown method");
            return null!;
        }
        using(var cmd = new MySqlCommand(@$"select * from book where {method}=@content",_db)){
        cmd.Parameters.AddWithValue("@content",content);
        using(var reader = cmd.ExecuteReader()){
            int count=reader.FieldCount;
            var results = new List<Dictionary<string,object>>();
            if(!reader.HasRows){
                return null!;
            }
            while(reader.Read()){
                var row = new Dictionary<string,object>();
                for(int e = 0; e < count; e++){
                    row[reader.GetName(e)]=reader.GetValue(e);
                }
                results.Add(row);
            }
            return results;
        }
    }   
}
        
        //public boolean queryUser(string user){
        //    using(var cmd = new MySqlCommand("select name from user",_db)){
        //        using(var reader = cmd.ExecuteReader()){
        //            int counter=reader.FieldCount;
        //            while(reader.Read()){
        //                if(reader.GetValue(0) == user){
        //                    return true;
        //                }
        //            }
        //            return null;
        //        }    
        //    }
        //}

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

            }
        }
        
    }
    public static class Idk{
        public static Database db = new Database();
    }
}

