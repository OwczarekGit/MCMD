using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using ForgedCurse.Enumeration;

namespace MCModDownloader
{
    public class ConsoleMenu
    {
        private List<ModItem> modList = new List<ModItem>();
        private bool running = true;
        private int selection = 0;

        public ConsoleMenu()
        {
        }
        

        public void searchMods()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Search:");
            var search = Console.ReadLine();

            var results = Program.client.SearchAddons(search, "", 10, 0, AddonKind.Mod);
            bool contains = false;
            foreach (var mod in results)
            {
                foreach (var ownedMod in modList)
                {
                    if (mod.Name.Equals(ownedMod.name))
                    {
                        contains = true;
                    }
                } 
                
                if (!contains) 
                    modList.Add(new ModItem(mod.Name, mod.Identifier, this));
            }
        }

        public void getInput()
        {
            drawMenu();
            while (running)
            {
                Console.CursorVisible = false;
                var input = Console.ReadKey().Key;
                
                if (input == ConsoleKey.DownArrow || input == ConsoleKey.J)
                {
                    selection = selection+1 > modList.Count-1 ? modList.Count-1 : selection+1;
                }

                if (input == ConsoleKey.UpArrow || input == ConsoleKey.K)
                {
                    selection = selection-1 < 0 ? 0 : selection-1;
                }

                if (input == ConsoleKey.F || input == ConsoleKey.Divide)
                {
                    searchMods();
                }

                if (input == ConsoleKey.Spacebar)
                {
                    modList[selection].toggleMark();
                }

                if (input == ConsoleKey.D)
                {
                    downloadSelected();
                }

                if (input == ConsoleKey.C)
                {
                    clearEntries();
                }

                if (input == ConsoleKey.Q)
                {
                    running = false;
                }

                if (input == ConsoleKey.O)
                {
                    var tmp = modList[selection];
                    var url = tmp.getModUrl();
                    Console.Clear();
                    Console.Write($"[{tmp.name}]: {url}");
                    var x = Console.ReadKey().Key;
                }
                
                drawMenu();
            }
            
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Bye!");
        }

        public void drawMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--- Menu ---");
            for (int i = 0; i < modList.Count; i++)
            {
                if (i == selection)
                {
                    Console.WriteLine("> "+modList[i].getMod());
                }
                else
                {
                    Console.WriteLine("  "+modList[i].getMod());
                }
            }
        }

        public void clearEntries()
        {
            List<ModItem> newModList = new List<ModItem>();
            
            foreach (var mod in modList)
            {
                if (mod.markedForDownload || mod.isDownloaded)
                    newModList.Add(mod);
            }

            modList = newModList;

            if (selection >= modList.Count)
                selection = modList.Count - 1;
        }

        public void downloadSelected()
        {
            Console.Clear();
            int counter = 0;
            List<Thread> downloadThreads = new List<Thread>();
            
            foreach (var mod in modList)
            {
                if (mod.markedForDownload)
                { 
                    downloadThreads.Add(new Thread(mod.downloadMod));
                    counter++;
                }
            }
            
            Console.WriteLine($"Starting download of {counter} mods.");
            Thread.Sleep(2000);

            foreach (var download in downloadThreads)
                download.Start();
            
            foreach (var download in downloadThreads)
                download.Join();

            Console.Clear();
            Console.WriteLine($"Finished downloading: {counter} mods!\nYou can find them in: {Program.workingDirectory}.\nPress any key to continue.");
            var tmp= Console.ReadKey().KeyChar;
        }
    }
}