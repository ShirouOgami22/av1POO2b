using static improving.Improveding;
using static extras.extraFucns; 
using Datas;
using static Datas.Idk;


using static vars.Thingyes;
using System.Text.RegularExpressions;
namespace Library{
    public static class LibraryFuncs{
        public static void select(){
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
                    try{ b = int.Parse(b!.ToString()!);}catch(Exception){
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
