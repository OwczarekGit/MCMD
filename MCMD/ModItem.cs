using System;
using System.Linq;
using System.Net;
using System.Threading;
using ForgedCurse;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    public class ModItem
    {
        public String name = "";
        private int modID = -1;
        public bool isDownloaded = false;
        public bool markedForDownload = false;
        private ConsoleMenu cmRef;

        public ModItem(String name, int modID, ConsoleMenu cr)
        {
            this.name = name;
            this.modID = modID;
            this.cmRef = cr;
        }

        public String getMod()
        {
            if (markedForDownload)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (isDownloaded)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    return $"[✓] {name}";
                }
                return $"[x] {name}";
            }

            Console.ForegroundColor = ConsoleColor.White;
            return $"[.] {name}";
        }

        public void toggleMark()
        {
            markedForDownload = !markedForDownload;
        }

        public void downloadMod()
        {
            if (isDownloaded)
                return;

            var tmpMod = Program.client.GetAddon(modID);
            var fileList = tmpMod.LatestFiles;
            CurseJSON.AddonFile target = fileList.Last();
            
            WebClient tmpClient = new WebClient();
            
            // Download the mod dependencies
            foreach (var dependency in target.dependencies)
            {
                if (dependency.type != 3) continue;
                
                var tmpDep = Program.client.GetAddon(dependency.addonId);
                var depList = tmpDep.LatestFiles;
                CurseJSON.AddonFile depTarget = depList.Last();
                    
                tmpClient.DownloadFile(depTarget.downloadUrl, Program.workingDirectory + depTarget.fileName);
            }
            tmpClient.DownloadFile(target.downloadUrl, Program.workingDirectory + target.fileName);

            isDownloaded = true;
            cmRef.drawMenu();
        }

        public String getModUrl()
        {
            return Program.client.GetAddon(modID).Website;
        }
    }
}