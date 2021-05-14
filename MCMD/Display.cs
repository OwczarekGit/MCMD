using System;
using System.Threading;
using ForgedCurse.Enumeration;

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
            buffer = new ConsolePanel(null, null);
            added = new ConsolePanel(null, null);
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
            String tmp = $" =[{Program.ReleaseVersion}]";
            for (int i = 0; i < size.x-Program.ReleaseVersion.Length-5; i++)
                tmp += "=";

            footer = tmp;
        }

        private void drawFooter()
        {
            Console.SetCursorPosition(0,size.y);
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

        private ConsoleKeyInfo getInput()
        {
            return Console.ReadKey(true);
        }

        public void processInput()
        {
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
            bool typing = true;
            
            while (typing)
            { 
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
                    typing = false;
                    focusBuffer();
                    currentState = AppStates.Browse;
                }
                else
                {
                    searchBuffer += input.KeyChar; 
                    buffer.barText = $"Search ({searchBuffer.Length})> {searchBuffer}";
                }

                draw();
            }
            
            draw();
        }

        private void browseModeInput()
        {
            while (currentState == AppStates.Browse)
            {
                var input = getInput();

                if (input.Key == ConsoleKey.J || input.Key == ConsoleKey.DownArrow)
                    focusedPanel.increseSelection();

                if (input.Key == ConsoleKey.K || input.Key == ConsoleKey.UpArrow)
                    focusedPanel.decreseSelection();
                
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
                    buffer.listItem.Add(tmp);
                }
                else
                {
                    buffer.listItem.Add(tmp);
                }
            }

            buffer.barText = $"Results ({buffer.selection+1}/{buffer.listItem.Count}): {name}";
            draw();
        }
    }
}