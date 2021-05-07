using System;
using System.Linq;
using System.Threading;
using ForgedCurse;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    class Program
    {
        public static ForgeClient client;
        public static String workingDirectory = null;
        public static String mcVersion = "1.16.5";
        
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You need to specify a working directory! (Please provide FULL path)");
                return;
            }

            workingDirectory = args[0] + "/";
            Console.WriteLine($"Targeting directory: {workingDirectory}");
            
            client = new ForgeClient();

            var modMenu = new ConsoleMenu();
            
            modMenu.searchMods();
            modMenu.getInput();
            
        }
    }
}