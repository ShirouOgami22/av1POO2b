using System.Text.RegularExpressions;
using static improving.Improveding;
using static Library.LibraryFuncs;
using static extras.extraFucns; 
using static vars.Thingyes;
using static Datas.Idk;
using Datas;
using System.Threading.Tasks;
namespace libN{
    public static class NoguiLibrary{
       public static void libraryNogui(){
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
                            list(a[1]);
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
    }
}