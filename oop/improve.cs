using static dbN.dbManager; 
using static libN.NoguiLibrary;
using static extras.extraFucns; 
using static vars.thingyes;
using static Library.LibraryFuncs;
using System.Text.RegularExpressions;
using static Datas.idk;

namespace improving{
    public static class improveding{
        public static void print(string a){
            Console.WriteLine(a);
        }
        public static string[]? input(string a){
        Console.Write(a);
        string? txt = Console.ReadLine();;
        if(isNull(txt!)){
            return Array.Empty<string>();
        }else{
             return Regex.Matches(txt!, @"(?:\"".*?\"")|(?:\S+)").Cast<Match>().Select(m => m.Value.Trim('"')).ToArray();
        }
    }
        public static bool isNull(string a){if(a==null||a==""){return true;}return false;}
    }    
}