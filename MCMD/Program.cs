using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using ForgedCurse;
using ForgedCurse.Enumeration;
using ForgedCurse.WrapperTypes;
using Microsoft.VisualBasic;

namespace MCModDownloader
{
    class Program
    { 
        public static readonly String ReleaseVersion = $"200521";
        public static ForgeClient client;
        public static String workingDirectory = null;
        public static String mcVersion = "1.16.5";
        public static String modLoader = "forge";
        
        static void Main(string[] args)
        {
            ArgumentResolver argumentResolver = new ArgumentResolver(args);
            
            if (args.Length < 1)
            {
                Console.WriteLine("You need to specify a working directory!");
                return;
            }

            workingDirectory = $"{Directory.GetCurrentDirectory()}/{args[0]}/";

            if (!Helpers.directoryExists(workingDirectory))
            {
                var shouldCreate = Helpers.getConfirmation($"Directory: '{workingDirectory}' doesn't exist, should it be created?");

                if (shouldCreate)
                {
                    var result = Helpers.createDirectory(workingDirectory);

                    if (!result)
                    {
                        Console.WriteLine($"Failed to create directory: {workingDirectory} exiting!");
                        Environment.Exit(-1);
                    }
                }
                else
                {
                    Console.WriteLine("Canceling.");
                    Environment.Exit(0);
                }
            }

            if (!Helpers.isDirectoryEmpty(workingDirectory))
            {
                // TODO check if files are mod files and if so set environment to work with them.
                Console.WriteLine("Directory contains files!");
            }
            
            Console.WriteLine($"Targeting directory: {workingDirectory}");
            
            client = new ForgeClient();
                
            Display display = new Display();
            display.start();
            while (true)
            {
                display.processInput();
            }
        }
    }
}