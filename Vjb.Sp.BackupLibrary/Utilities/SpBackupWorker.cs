using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vjb.Sp.BackupLibrary.Model;

namespace Vjb.Sp.BackupLibrary.Utilities
{
    public class SpBackupWorker
    {
        /*
        public void doWork(object sender, DoWorkEventArgs e)
        {
            var libInfo = e.Argument as LibraryInformation;

            if (libInfo == null) return;
            List<string> result = new List<string>();

            var sharepointClient = libInfo.SharepointClient;
            sharepointClient.OnProgressUpdate += this.sharepointClient_OnProgressUpdate;
            sharepointClient.OnErrorHandler += sharepointClient_ErrorHandler;
            var rootFolder = sharepointClient.GetRootFolder(libInfo.ListName);
            var folders = sharepointClient.GetFolders(rootFolder);

            sharepointClient.ProcessFolder(libInfo.DestinationFolder, rootFolder);
        }

        private void sharepointClient_OnProgressUpdate(FileDownloadStatus status)
        {
            throw new NotImplementedException();
        }

        public void sharepointClient_ErrorHandler(Exception exception, TextBox textBox)
        {
            base.Invoke((Action)delegate
            {
                textBox.AppendText(string.Format("The operation failed: {0}\r\nStacktrace: {1}", exception.Message, exception.StackTrace));
            });
        }
        
        public void sharepointClient_OnProgressUpdate(FileDownloadStatus status, TextBox textBox)
        {
            base.Invoke((Action)delegate
            {
                string message = string.Empty;
                switch (status.FileType)
                {
                    case FileType.Folder:
                        var output = string.Format("\r\nProcessing the folder '{0}', Folders: {1}, Files: {2}\r\n", status.FileName, status.FolderCount, status.FilesCount);
                        message = string.Format("{0}{1}\r\n", output, new string('*', (output.Length) + 10));
                        files.Add(string.Format("{0};{1};Folders: {2} Files: {3}", FileType.Folder, status.FileName, status.FolderCount, status.FilesCount));
                        break;
                    case FileType.File:
                        message = string.Format("Downloading '{0}', to {1}", status.FileName, status.Destination);
                        files.Add(string.Format("{0};{1};{2}", FileType.File, status.FileName, status.Destination));
                        break;
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    textBox.AppendText(message + "\r\n");
                }
            });
        }
        
        public void bwCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                tbOutput.AppendText("\r\n\r\nFinished!");
            }
            else
            {
                tbOutput.AppendText(string.Format("\r\n\r\nFailure: {0}\r\n{1}", e.Error.Message, e.Error.StackTrace));
            }
            btnCopyToClipboard.Enabled = true;
            btnCsv.Enabled = true;
        }
    
         */
}}
