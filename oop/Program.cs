using static dbN.dbManager; 
using static libN.NoguiLibrary;
using static extras.extraFucns; 
using static vars.thingyes;
using static improving.improveding;
using static Library.LibraryFuncs;
using Datas{
    public static class Globals
    {
        public static Database db = new Database();
    }
}

var builder = WebApplication.CreateBuilder(args);
print("idk?");

builder.Services.AddOpenApi();
var app = builder.Build();


app.Run();