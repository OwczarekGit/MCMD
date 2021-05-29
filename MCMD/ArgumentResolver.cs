using System;
using System.Collections.Generic;

namespace MCModDownloader
{
    public class ArgumentResolver
    {
        public List<String> arguments = new List<string>();
        public ArgumentResolver(String[] args)
        {
            foreach (var arg in args)
                arguments.Add(arg);
            
            processArguments();
        }

        private void processArguments()
        {
            try
            {
                for (int i=0; i < arguments.Count; i++)
                {
                    String currentArg = arguments[i];

                    if (currentArg == "--help" || currentArg == "-h")
                    {
                        printUsage();
                        Environment.Exit(0);
                    }

                    if (currentArg == "--version" || currentArg == "-v")
                        Program.mcVersion = arguments[i + 1];

                    if (currentArg == "--loader" || currentArg == "-l")
                        Program.modLoader = arguments[i + 1];


                }
            }
            catch(Exception e){ printUsage(); }
        }

        public void printUsage()
        {
            Console.WriteLine("Usage: mcmd path/to/mods/directory [options]\n");
            Console.WriteLine("Note that path MUST be first argument.\nOptions:");
            Console.WriteLine("--version MinecraftVersion, -v MinecraftVersion - Set minecraft version that you want to use. (1.16.5 by default)");
            Console.WriteLine("--loader ModLoader, -l ModLoader - Set a mod loader that you want to use. (Forge by default)");
            Console.WriteLine("--help, -h - Print this message.");
        }
    }
}