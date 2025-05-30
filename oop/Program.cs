using System.Text.RegularExpressions;
using static improving.Improveding;
using static Controller.Controls;
using static Library.LibraryFuncs;
using static extras.extraFucns; 
using static libN.NoguiLibrary;
using static vars.Thingyes;
using static Datas.Idk;
using Datas;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); 
var app = builder.Build();
static bool login(){
    while(true){
            string user=string.Join(" ",input("USER: ")!);
            if(user.Length==0||user==null){
                print("Type something");
            }else{
                Console.Clear();
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
                return false;
            }
        }
        if(permissions=="manager"){
            return true;
        }else{
            print($"You, {worker}, dont have direct acess to the database");
            return false;
    }

}
db.connect();
if(args.Length<=0){
    print("Please state whether you wish to manage the database with 'database' or start the program with 'library'\nYou may choose wheter to use a graphic user interface: 'noGUI' or 'GUI'");
    print("'<program>.exe library nogui'");
}else{
    if(args[0].ToLower()=="library" && args[1].ToLower()=="nogui"){
        libraryNogui();
    }else if(args[0].ToLower()=="library" && args[1].ToLower()=="gui"){
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapFallbackToFile("/login/login.html");
        app.MapControllers();
        app.Run();
    }else if(args[0].ToLower()=="database" && args[1].ToLower()=="nogui"){
        login();
    }else if(args[0].ToLower()=="database" && args[1].ToLower()=="gui"){
        //
    }else{
        print("Unknown arguments\nexample of correct use:\n<program>.exe library nogui");
    }
}
