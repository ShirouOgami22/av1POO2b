using static libN.NoguiLibrary;
using static extras.extraFucns; 
using static vars.thingyes;
using static improving.improveding;
using static Library.LibraryFuncs;
using static Datas.idk;
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
}
