using System;
using System.Collections.Generic;

namespace MCModDownloader
{
    public class ConsolePanel
    {
        private static class ModStatus
        {
            public static ConsoleColor downloaded = ConsoleColor.Green;
            public static ConsoleColor marked = ConsoleColor.Yellow;
            public static ConsoleColor error = ConsoleColor.Red;
            public static ConsoleColor normal = ConsoleColor.White;
        }
        
        public Vec2 size { set; get; }
        public Vec2 position { set; get; }
        public List<Mod> listItem { private set; get; }
        public int offset { private set; get; } = 0;
        public int selection { private set; get; } = 0;
        public String barText = $"Panel";

        public ConsolePanel(Vec2 position, Vec2 size)
        {
            this.size = size;
            this.position = position;
            listItem = new List<Mod>();
        }

        public void draw()
        {
            String titleBarText = barText;
            for (int j = 0; j < size.x-barText.Length; j++)
            {
                titleBarText += "=";
            }
            
            int i = 0;

            Console.ForegroundColor = ModStatus.normal;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write($"{titleBarText}");
            Mod tmpItem = null;
            
            while (offset+i+selection < offset+selection+size.y-1)
            {
                if (offset+i > listItem.Count-1)
                    break;
                    
                
                tmpItem = listItem[offset+i];
                
                if (tmpItem.isDownloaded)
                {
                    Console.ForegroundColor = ModStatus.downloaded;
                }
                else if (tmpItem.isMarked)
                {
                    Console.ForegroundColor = ModStatus.marked;
                }
                else
                {
                    Console.ForegroundColor = ModStatus.normal;
                }

                Console.SetCursorPosition(position.x, position.y+1 + i);

                tmpItem.updateDisplayName();
                String tmpString = tmpItem.displayName;
                String composedString = "";
                bool wasTooLong = false;
                
                for (int j = 0; j < tmpString.Length; j++)
                {
                    if (j <= size.x-2-4)
                    {
                        composedString += tmpString[j];
                    }
                    else
                    {
                        wasTooLong = true;
                    }
                }

                if (wasTooLong)
                    composedString += '…';
                
                
                if (selection == i+offset)
                {
                    Console.Write($" >{composedString}");
                }
                else
                {
                    Console.Write($"  {composedString}");
                }
                // ⭳ ✓
                i++;
            }
        }

        public void increseSelection()
        {
            if (selection < listItem.Count-1) 
                selection++;
            
            if (selection + offset >= size.y-2)
                increseOffset();
        }

        public void decreseSelection()
        {
            if (selection > 0)
                selection--;

            if (selection - offset < 1)
                decreseOffset();
        }
        
        public void increseOffset()
        {
            if(offset < listItem.Count-size.y+1)
                offset++;
        }

        public void decreseOffset()
        {
            if(offset > 0)
                offset--;
        }

        public void toggleMark()
        {
            if (!listItem[selection].isDownloaded)
            {
                listItem[selection].isMarked = !listItem[selection].isMarked;
            }
        }
    }
}