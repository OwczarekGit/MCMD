using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using ForgedCurse;

namespace MCModDownloader
{
    public class DownloadInstance
    {
        private Uri url;
        private String fileName;
        public bool downloadWasSuccess { private set; get; }
        private Thread downloadThread;
        public byte downloadProgress { private set; get; }
        private WebClient client;
        private Mod modRef; 

        public DownloadInstance(CurseJSON.AddonFile targetFile, Mod modRef)
        {
            this.fileName = targetFile.fileName;
            this.url = new Uri(targetFile.downloadUrl);
            this.modRef = modRef;
            downloadProgress = 0;
            downloadThread = new Thread(beginDownload);
        }

        public void startDownload()
        {
            downloadThread.Start();
        }

        public bool downloadFinished()
        {
            downloadThread.Join();
            return downloadWasSuccess;
        }

        private void beginDownload()
        {
            client = new WebClient();
            client.DownloadProgressChanged += updateDownloadProgress;
            client.DownloadFileCompleted += downloadFinished;
            client.DownloadFileAsync(url, Program.workingDirectory + fileName);
        }


        private void updateDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            modRef.updateDisplayName();
            downloadProgress = (byte)e.ProgressPercentage;
        }

        private void downloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
            {
                modRef.isDownloaded = false;
                downloadWasSuccess = false;
            }

            modRef.isDownloaded = true;
            downloadWasSuccess = true;
            modRef.displayName = modRef.addon.Name;
        }
        
    }
}