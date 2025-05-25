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
            using(var command = new MySqlCommand($"select column_name from information_schema.columns where table_name = @table and column_name = @columnName", _db)){
            command.Parameters.AddWithValue("@table", table);
            command.Parameters.AddWithValue("@columnName", columnName);
            using(var reader = command.ExecuteReader()){
                while (reader.Read()){
                    for(int i=0;i<count;i++){
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
        worker="";
        permissions="";
        using(var cmd = new MySqlCommand($"select * from user where name like @a;",_db)){
        cmd.Parameters.AddWithValue("@a", $"%{a}%");
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

        public object? querry(string mode, string met="id", string ord="asc"){
            string comando;
            //print($"{mode}, {met}, {ord}");
            ord=ord.ToLower();
            if(!(met=="id"||met=="pubYear"||met=="author"||met=="title")){
                print("failed met\nGoing by id");
                met="id";
            }else if(!(ord=="asc"||ord=="desc")){
                print("failed ord\nGoing ascending");
                ord="asc";
            }
            if(mode=="all"){
                comando=$"select * from book order by {met} {ord}";
            }else if(mode=="available"){
                comando=$"select * from book where available=1 order by {met} {ord}";
            }else if(mode=="unavailable"){
                comando=$"select * from book where available=0 order by {met} {ord}";
            }else{
                print("Unknown mode");
                return null!;
            }
            List<Dictionary<string,object>> results = new List<Dictionary<string,object>>();
            using(var cmd = new MySqlCommand(comando,_db)){
                cmd.Parameters.AddWithValue("@met",met);
                cmd.Parameters.AddWithValue("@ord",ord);
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
                }}
            return results;
       }
    
        public object? querryUsers(string mode){
            if(mode!="all"){
                return null;
            }
                List<Dictionary<string,object>> results = new List<Dictionary<string,object>>();
                using(var cmd = new MySqlCommand("select * from `user`",_db))
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
        
        public object querry(string method,object content, string met="id", string ord="asc"){
        string command;
        print($"{method}, {content}, {met}, {ord}, here");
        if(!checkTableCols(method)){
            print("Unknown method");
            return null!;
        }
        ord=ord.ToLower();
        if(!(met=="id"||met=="pubYear"||met=="author"||met=="title")){
            print("failed met\nGoing by id");
            met="id";
        }else if(!(ord=="asc"||ord=="desc")){
            print("failed ord\nGoing ascending");
            ord="asc";
        }
        if(method=="id"||method=="pubYear"){
            command=@$"select * from book where {method}=@content";
        }else{
            command=@$"select * from book where {method} like @content order by {met} {ord}";
        }
        using(var cmd = new MySqlCommand(command,_db)){
            if(method=="id"||method=="pubYear"){
                cmd.Parameters.AddWithValue("@content",content);
            }else{
                cmd.Parameters.AddWithValue("@content",$"%{content}%");
            }
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

        public void create(string what,string title,string author,int? pubyear,string category){
            if(!checkTableCols("id",what)){
                print($"table {what} doesnt exist");
                return;
            }
            if(
                isNull(title)||
                isNull(author)||
                pubyear==null||
                isNull(category)
            ){print($"some field is null");return;}
            try{using(var cmd=new MySqlCommand($"insert into {what}(author,title,pubYear,category) values (@A,@T,@P,@C); ",_db)){
                cmd.Parameters.AddWithValue("T",title);
                cmd.Parameters.AddWithValue("A",author);
                cmd.Parameters.AddWithValue("P",pubyear);
                cmd.Parameters.AddWithValue("C",category);
                //cmd.Parameters.AddWithValue("what",what);
                //uhh, tho its unsafe, i guess the easiest way is to directly insert 'what' into the command
                cmd.ExecuteNonQuery();
            }}catch(Exception err){print($"some error happened: {err}");return;}
        }

        public void editBook(int? id,string title,string author,int? pubYear,string category){
        
        try{using(var cmd=new MySqlCommand($"UPDATE book set title=@T,author=@A,pubYear=@P,category=@C where id={id}",_db)){
                cmd.Parameters.AddWithValue("T",title);
                cmd.Parameters.AddWithValue("A",author);
                cmd.Parameters.AddWithValue("P",pubYear);
                cmd.Parameters.AddWithValue("C",category);
                cmd.ExecuteNonQuery();
            }}catch{return;}
        }

        public void remove(int id){
            using(var cmd = new MySqlCommand("delete from book where id=@id",_db)){
                cmd.Parameters.AddWithValue("id",id);
                cmd.ExecuteNonQuery();
            }
    }

    }
    public static class Idk{
        public static Database db = new Database();
    }
}

