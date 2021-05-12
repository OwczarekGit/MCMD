using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ForgedCurse;
using ForgedCurse.Enumeration;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    class Program
    {
        public static ForgeClient client;
        public static String workingDirectory = null;
        public static String mcVersion = "1.16.5";
        public static String modLoader = "forge";
        public static Vec2 screenSize = new Vec2(0, 0);
        
        static void Main(string[] args)
        {
            screenSize.x = Console.WindowWidth;
            screenSize.y = Console.WindowHeight;

            /*if (args.Length < 1)
            {
                Console.WriteLine("You need to specify a working directory! (Please provide FULL path)");
                return;
            }

            workingDirectory = args[0] + "/";
            Console.WriteLine($"Targeting directory: {workingDirectory}");
            
            client = new ForgeClient();

            var modMenu = new ConsoleMenu();
            
            modMenu.searchMods();
            modMenu.getInput();*/

            Console.CursorVisible = false;

            ConsolePanel panel = new ConsolePanel(new Vec2(1, 1),new Vec2(40, 10));
            client = new ForgeClient();
                
            
            var search = client.SearchAddons("ref", "1.16.5", 40, 0, AddonKind.Mod);

            foreach (var addon in search)
            {
                Mod tmpMod = new Mod(addon);
                
                if (modLoader == "forge" && tmpMod.loaderTypeForge)
                {
                    panel.listItem.Add(tmpMod);
                    //tmpMod.getAddonFile();
                }
                else if (modLoader == "fabric" && !tmpMod.loaderTypeForge)
                {
                    panel.listItem.Add(tmpMod);
                    //tmpMod.getAddonFile();
                }
            }

            //Console.ReadKey();

            
            while (true)
            {
                Console.Clear();
                panel.draw();
                var input = Console.ReadKey().Key;

                if (input == ConsoleKey.J)
                    panel.increseSelection();

                if (input == ConsoleKey.K)
                    panel.decreseSelection();

                if (input == ConsoleKey.Spacebar)
                    panel.toggleMark();

                if (input == ConsoleKey.RightArrow)
                    panel.position.x++;

                if (input == ConsoleKey.LeftArrow)
                    panel.position.x--;

                if (input == ConsoleKey.DownArrow)
                    panel.position.y++;
                
                if (input == ConsoleKey.UpArrow)
                    panel.position.y--;
                
            }
        }
    }
}