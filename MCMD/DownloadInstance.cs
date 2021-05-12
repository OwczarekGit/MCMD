using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

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

        public DownloadInstance(String url, String fileName, Mod modRef)
        {
            this.fileName = fileName;
            this.url = new Uri(url);
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
            client.DownloadFile(url, fileName);
        }

        private void updateDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
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
        }
        
    }
}