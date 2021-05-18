using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using ForgedCurse;
using ForgedCurse.Enumeration;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    public class Display
    {
        public Vec2 size = new Vec2(0,0);
        private Thread sizeUpdater;
        private ConsolePanel buffer;
        private ConsolePanel added;
        private bool running = false;
        private String footer = "";
        private AppStates currentState = AppStates.Search;
        private String searchBuffer = "";
        private ConsolePanel focusedPanel = null;

        public Display()
        {
            buffer = new ConsolePanel(new Vec2(1,1), new Vec2(1,1));
            added = new ConsolePanel(new Vec2(1,1), new Vec2(1,1));
            getModsFromExistingFiles();
            focusBuffer();
            
            updateSize();
            sizeUpdater = new Thread(updateSize);
        }

        public void start()
        {
            if (running)
                return;
            
            sizeUpdater.Start();
            running = true;
        }

        private void getModsFromExistingFiles()
        {
            var files = Directory.GetFiles(Program.workingDirectory,"*.jar");
            Console.WriteLine($"Found {files.Length} existing mods. Working on them...");

            foreach (var file in files)
            {
                Addon addon = Program.client.GetAddonFromFile($"{file}");

                if (addon != null)
                {
                    var localMod = new Mod(addon);
                    added.listItem.Add(localMod);
                }
            }
            
            added.checkForUpdates();
        }

        public void updateSize()
        {
            while (running)
            {
                Vec2 prevSize = new Vec2(size.x, size.y);
                
                size.x = Console.WindowWidth;
                size.y = Console.WindowHeight;

                if (prevSize.isChanged(size))
                { 
                    updatePanelSize(); 
                    updateFooter();
                    draw();
                }
                
                Thread.Sleep(100);
            }
        }

        private void updatePanelSize()
        {
            buffer.position = new Vec2(1, 0);
            buffer.size = new Vec2(size.x/2-2, size.y-1);
            
            added.position = new Vec2(size.x/2+1, 0);
            added.size = new Vec2(size.x/2-2, size.y-1);
        }

        private void updateFooter()
        {
            String footerText = $" =[{Program.ReleaseVersion}]=[{Program.mcVersion}/{Program.modLoader}]";
            String tmp = footerText;
            for (int i = 0; i < size.x-footerText.Length-1; i++)
                tmp += "=";

            footer = tmp;
        }

        private void drawFooter()
        {
            Helpers.setCursor(0,size.y-1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(footer);
        }

        public void draw()
        {
            Console.Clear();
            buffer.draw();
            added.draw();
            drawFooter();
        }
        
        private enum AppStates 
        {
            Search,
            Browse
        }

        private void focusBuffer()
        {
            focusedPanel = buffer;
            buffer.isFocused = true;
            added.isFocused = false;
        }

        private void focusAdded()
        {
            focusedPanel = added;
            buffer.isFocused = false;
            added.isFocused = true;
        }

        private void togglePanelFocus()
        {
            if (focusedPanel == buffer)
                focusAdded();
            else
                focusBuffer();
        }

        private ConsoleKeyInfo getInput()
        {
            return Console.ReadKey(true);
        }

        public void processInput()
        {
            draw();
            switch (currentState)
            {
                case AppStates.Browse:
                    browseModeInput();
                    break;
                
                case AppStates.Search:
                    searchModeInput();
                    break;
            }
            
        }

        private void searchModeInput()
        {
            buffer.barText = $"Search ({searchBuffer.Length})> {searchBuffer}";
            searchBuffer = "";
            draw();
            
            while (currentState == AppStates.Search)
            { 
                buffer.barText = $"Search ({searchBuffer.Length})> {searchBuffer}";
                var input = getInput();
                
                if (input.Key == ConsoleKey.Backspace && searchBuffer.Length > 0)
                {
                    searchBuffer = searchBuffer.Remove(searchBuffer.Length - 1);
                }
                else if (input.Key == ConsoleKey.Delete)
                {
                    searchBuffer = "";
                }
                else if (input.Key == ConsoleKey.Enter && searchBuffer.Length > 0)
                {
                    performModSearch(searchBuffer);
                    focusBuffer();
                    currentState = AppStates.Browse;
                }
                else
                {
                    searchBuffer += input.KeyChar; 
                }

                buffer.barText = $"Search ({searchBuffer.Length})> {searchBuffer}";
                draw();
            }
            
            draw();
        }

        private void browseModeInput()
        {
            focusedPanel.barText = $"Position: ({focusedPanel.selection+1}/{focusedPanel.listItem.Count})";
            draw();
            
            while (currentState == AppStates.Browse)
            { 
                focusedPanel.barText = $"Position: ({focusedPanel.selection+1}/{focusedPanel.listItem.Count})";
                var input = getInput();

                if (input.Key == ConsoleKey.J || input.Key == ConsoleKey.DownArrow)
                    focusedPanel.increseSelection();

                if (input.Key == ConsoleKey.K || input.Key == ConsoleKey.UpArrow)
                    focusedPanel.decreseSelection();

                if (input.Key == ConsoleKey.Spacebar)
                    focusedPanel.toggleMark();
                
                if(input.Key == ConsoleKey.Tab)
                    togglePanelFocus();

                if (input.Key == ConsoleKey.F)
                { 
                    currentState = AppStates.Search;
                    moveBuffer();
                    moveAdded();
                    focusBuffer();
                }

                if (input.Key == ConsoleKey.C) 
                    clearPanel(focusedPanel);

                if (focusedPanel == added && input.Key == ConsoleKey.D)
                    downloadMods();

                if (input.Key == ConsoleKey.O)
                {
                    String url = focusedPanel.getSelectedURL();
                    
                    if (url != null)
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Process.Start("explorer", url);
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            Process.Start("xdg-open", url);
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            Process.Start("open", url);
                        
                        draw();
                    }
                }

                if (input.Key == ConsoleKey.Q && (input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    running = false;
                    Console.CursorVisible = true;
                    Console.Clear();
                    Console.WriteLine("Bye!");
                    System.Environment.Exit(0);
                }
                    

                focusedPanel.barText = $"Position: ({focusedPanel.selection+1}/{focusedPanel.listItem.Count})";
                draw();
            }
            
            draw();
        }

        private void performModSearch(String name)
        {
            var searchResult = Program.client.SearchAddons(name, Program.mcVersion, 40, 0, AddonKind.Mod);
            foreach (var mod in searchResult)
            {
                Mod tmp = new Mod(mod);

                if (Program.modLoader == "forge" && tmp.loaderTypeForge)
                {
                    if (!containsMod(tmp, added)) 
                        exclusiveAdd(tmp, buffer);
                }
                else if (Program.modLoader == "fabric" && !tmp.loaderTypeForge)
                {
                    if (!containsMod(tmp, added)) 
                        exclusiveAdd(tmp, buffer);
                }
            }

            buffer.barText = $"Results ({buffer.selection+1+buffer.selection}/{buffer.listItem.Count}): {name}";
            draw();
        }

        private void exclusiveAdd(Mod mod, ConsolePanel panel)
        {
            bool exists = false;
            
            foreach (var item in panel.listItem)
            {
                if (item.addon.Name == mod.addon.Name) 
                    exists = true;
            }

            if (!exists)
                panel.listItem.Add(mod);
        }

        private bool containsMod(Mod mod, ConsolePanel panel)
        {
            foreach (var tmpMod in panel.listItem)
                if (tmpMod.addon.Name == mod.addon.Name)
                    return true;

            return false;
        }

        private void moveBuffer()
        {
            List<Mod> removeList = new List<Mod>();
            
            foreach (var mod in buffer.listItem)
            {
                if (mod.isMarked)
                {
                    exclusiveAdd(mod, added);
                    removeList.Add(mod);
                }
            }

            foreach (var mod in removeList)
            {
                buffer.listItem.Remove(mod);
            }
            
            buffer.selectMax();
        }

        private void moveAdded()
        {
            List<Mod> removeList = new List<Mod>();

            foreach (var mod in added.listItem)
            {
                if (!mod.isMarked)
                {
                    exclusiveAdd(mod, buffer);
                    removeList.Add(mod);
                }
            }

            foreach (var mod in removeList)
            {
                added.listItem.Remove(mod);
            }
            
            added.selectMax();
        }

        private void clearPanel(ConsolePanel panel)
        { 
            List<Mod> removeList = new List<Mod>();

            foreach (var mod in panel.listItem)
            {
                if (!mod.isMarked || mod.isDownloaded)
                    removeList.Add(mod);
            }

            foreach (var mod in removeList)
            {
                panel.listItem.Remove(mod);
            }

            panel.selectMax();
        }

        private void downloadMods()
        {
            List<Thread> downloadThreads = new List<Thread>();
                
            foreach (var mod in added.listItem)
            {
                if (!mod.isDownloaded && mod.isMarked)
                    downloadThreads.Add(new Thread(mod.downloadMod));
            }

            foreach (var thread in downloadThreads)
            {
                thread.Start(); 
            }

            bool downloading = true;
            while (downloading)
            {
                int downloaded = 0;
                downloading = false;
                
                foreach (var mod in added.listItem)
                {
                    if (!mod.isDownloaded)
                        downloading = true;
                    else
                        downloaded++;
                }
                
                Thread.Sleep(100);
                added.barText = $"Downloading... ({downloaded}/{added.listItem.Count})";
                draw();
            }
            added.barText = $"Finished download of: {added.listItem.Count} mods.";
        }
    }
}