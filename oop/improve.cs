using static vars.Thingyes;
using System.Text.RegularExpressions;
namespace improving{
    public static class Improveding{
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
    public static bool isNull(string a) => string.IsNullOrEmpty(a);
    }    
}