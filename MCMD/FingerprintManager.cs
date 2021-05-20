using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    public class FingerprintManager
    {
        private String scanPath = "";
        private List<Thread> workerThreads = new List<Thread>();
        private List<FingerprintInstance> fingerprintInstances = new List<FingerprintInstance>();
        private List<Addon> computedAddons = new List<Addon>();
        private int totalFiles = 0;
        
        private class FingerprintInstance
        {
            private String fileName = "";
            public Addon? result = null;
            
            public FingerprintInstance(String fileName)
            {
                this.fileName = fileName;
            }

            public void calculateFingerprint()
            {
                Addon tmp = Program.client.GetAddonFromFile(fileName);
                result = tmp;
            }

            public Addon getResult()
            {
                return result ?? null;
            }
        }
        
        public FingerprintManager(String path)
        {
            scanPath = path;
            initInstances();
            startFingerprinting();
        }

        private void initInstances()
        {
            var files = Directory.GetFiles(scanPath, "*.jar");

            foreach (var file in files)
            {
                fingerprintInstances.Add(new FingerprintInstance(file));
                totalFiles++;
            }

            Console.WriteLine($"Found: {totalFiles} files.");
        }

        private void startFingerprinting()
        {
            Console.WriteLine($"Starting computing of {totalFiles} files.");
            foreach (var instance in fingerprintInstances)
                workerThreads.Add(new Thread(instance.calculateFingerprint));

            foreach (var thread in workerThreads)
                thread.Start();
            
            foreach (var thread in workerThreads)
                thread.Join();

            foreach (var instance in fingerprintInstances)
            {
                var tmpResult = instance.getResult();
                if (tmpResult != null) 
                    computedAddons.Add(tmpResult);
            }

            Console.WriteLine($"Successfully computed {computedAddons.Count} Mods.");
        }

        public List<Addon> getComputedAddons()
        {
            return computedAddons;
        }
        
        
    }
}