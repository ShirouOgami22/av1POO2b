using static dbN.dbManager; 
using static libN.NoguiLibrary;
using static extras.extraFucns; 
using static vars.thingyes;
using static improving.improveding;
using static Datas.idk;

using System.Text.RegularExpressions;
namespace Library{
       public static class LibraryFuncs{
       public static void select(){
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
       public static void search(string a = "default", object b = null!) {
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

    }
}
//class Program{
//
//    static void Main(string[] args){
//        //Console.Clear();
//        db.connect();
//        if(args.Length==0){
//            print("Please state whether you wish to manage the database with 'database' or start the program with 'library'\nYou may choose wheter to use a graphic user interface: 'noGUI' or 'GUI'");
//            print("<program>.exe library nogui");
//        }else{
//            if(args[0].ToLower()=="library" && args[1].ToLower()=="nogui"){
//                libraryNogui();
//            }else if(args[0].ToLower()=="library" && args[1].ToLower()=="gui"){
//                //
//            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="nogui"){
//                //attempt connection to an account in the database
//                while(true){
//                    string user=string.Join(" ",input("USER: ")!);
//                    if(user.Length==0){
//                        print("Type something");
//                    }else{
//                        if(db.logIn(user)){
//                            break;
//                        }
//                        print($"No user found with the name {user}");
//                    }
//                }
//                while(true){
//                    string password=string.Join(" ",input("PSWD: ")!);
//                    if(!isNull(worker)){
//                        if(db.paswd(worker,password)){
//                            print($"Logged in as {worker}!");
//                            break;
//                        }
//                    }else{
//                        print("user not selected\nTerminating program");
//                        return;
//                    }
//                }
//                if(permissions=="manager"){
//                    databaseNogui();
//                }else{
//                    print($"You, {worker}, dont have direct acess to the database");
//                }
//            }else if(args[0].ToLower()=="database" && args[1].ToLower()=="gui"){
//                //
//            }else{
//                print("Unknown arguments\nexample of correct use:\n<program>.exe library nogui");
//            }
//
//        }
//    }
//}
//