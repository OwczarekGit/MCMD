using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using ForgedCurse;
using ForgedCurse.Enumeration;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    class Program
    { 
        public static readonly String ReleaseVersion = $"MCMD Release: Rolling";
        public static ForgeClient client;
        public static String workingDirectory = null;
        public static String mcVersion = "1.16.5";
        public static String modLoader = "forge";
        
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You need to specify a working directory! (Please provide FULL path)");
                return;
            }

            workingDirectory = args[0] + "/";
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