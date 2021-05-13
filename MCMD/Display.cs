using System;
using System.Threading;

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

        public Display()
        {
            buffer = new ConsolePanel(null, null);
            added = new ConsolePanel(null, null);
            
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
                size.x = Console.WindowWidth;
                size.y = Console.WindowHeight;

                updatePanelSize();
                updateFooter();
                Thread.Sleep(100);
            }
        }

        private void updatePanelSize()
        {
            buffer.position = new Vec2(1, 0);
            buffer.size = new Vec2(size.x/2-2, size.y);
            
            added.position = new Vec2(size.x/2+1, 0);
            added.size = new Vec2(size.x/2-2, size.y);
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
    }
}