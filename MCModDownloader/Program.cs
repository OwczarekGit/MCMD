using System;
using System.Linq;
using ForgedCurse;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    class Program
    {
        public static ForgeClient client;
        public static String workingDirectory = null;
        
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You need to specify a working directory! (Please provide FULL path)");
                return;
            }

            workingDirectory = args[0];
            
            client = new ForgeClient();

            var modMenu = new ConsoleMenu();
            
            modMenu.searchMods();
            modMenu.getInput();
            
        }
    }
}