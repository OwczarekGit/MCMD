using System;
using System.IO;

namespace MCModDownloader
{
    public static class Helpers
    {
        public static bool directoryExists(String path)
        {
            if (Directory.Exists(path))
                return true;

            return false;
        }

        public static bool createDirectory(String path)
        {
            var result = Directory.CreateDirectory(path);

            if (directoryExists(path))
                return true;

            return false;

        }

        public static bool getConfirmation(String question)
        {
            Console.Write($"{question}(Y/N): ");
            var answer = Console.ReadKey();
            Console.WriteLine();
            
            while (answer.Key != ConsoleKey.N && answer.Key != ConsoleKey.Y)
            {
                Console.Write($"{question}(Y/N): ");
                answer = Console.ReadKey(); 
                Console.WriteLine();
            }

            if (answer.KeyChar.ToString().ToLower() == "y")
                return true;

            return false;
        }

        public static bool isDirectoryEmpty(String path)
        {
            if (Directory.GetFiles(path).Length > 0)
                return false;
            
            return true;
        }

        public static bool setCursor(int x, int y)
        {
            try
            {
                Console.SetCursorPosition(x,y);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}