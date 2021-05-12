using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ForgedCurse;
using ForgedCurse.Enumeration;
using ForgedCurse.WrapperTypes;

namespace MCModDownloader
{
    public class Mod 
    {
        private DownloadInstance downloadInstance;
        public bool isDownloaded = false;
        public bool isMarked = false;
        public Addon addon;
        public CurseJSON.AddonFile addonFile;
        private bool fileIsReady = false;
        public bool loaderTypeForge { private set; get; } = true;

        public Mod(Addon addon)
        {
            this.addon = addon;
            isForgeMod();
        }
        
        public String getModURL(){ return addon.Website; }

        public void downloadMod()
        {
            getAddonFile();
            getDownloadInstance();
            downloadInstance.startDownload();
        }

        private void getDownloadInstance()
        {
            downloadInstance = new DownloadInstance(addonFile.downloadUrl, addonFile.fileName, this);
        }

        private void getAddonFile()
        {
            List<CurseJSON.AddonFile> candidates = new List<CurseJSON.AddonFile>();
            foreach (var file in addon.Files)
            {
                if (file.Version == Program.mcVersion)
                {
                    CurseJSON.AddonFile fileCandidate = file.GetAddonFile(addon.Identifier);

                    if (Program.modLoader == "forge")
                    { 
                        if (fileCandidate.fileName.ToLower().Contains("fabric")) 
                            continue;
                    }
                    else
                    {
                        if (fileCandidate.fileName.ToLower().Contains("forge"))
                            continue;
                    }
                    
                    candidates.Add(fileCandidate);
                }
            }

            var targetFile = getNewestFile(candidates);
            addonFile = targetFile;
        }

        private CurseJSON.AddonFile getNewestFile(List<CurseJSON.AddonFile> candidates)
        {
            DateTime tmpDate = new DateTime(0);
            CurseJSON.AddonFile newest = null;
            foreach (var candidate in candidates)
            {
                if (candidate.fileDate > tmpDate)
                {
                    tmpDate = candidate.fileDate;
                    newest = candidate;
                }
            }

            return newest;
        }

        private void isForgeMod()
        {
            CurseJSON.AddonInfo addonInfo = addon;
            foreach (var category in addonInfo.categories)
                if (category.categoryId == 4780)
                {
                    loaderTypeForge = false;
                    return;
                }

            loaderTypeForge = true;
        }
    }
}