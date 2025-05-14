using static improving.Improveding;
using Datas;
using static Datas.Idk;

using static vars.Thingyes;
using System.Text.RegularExpressions;
namespace dbN{
    public static class dbManager{
        public static void databaseNogui(){
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
                        //query
                    break;
                    //
                    case "list":
                        //list
                    break;
                    //
                    case "create":
                        if(a.Length<2 || a.Length>2){
                            print("Invalid amount of arguments for 'create'");
                            print("You may create one of:");
                            print("book, user");//could be changed for a query... nahh
                        }else{
                            
                        }
                    break;
                    //
                    case "drop":
                        //drop something
                    break;
                    //
                    case "update":
                        //update a value
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
}
