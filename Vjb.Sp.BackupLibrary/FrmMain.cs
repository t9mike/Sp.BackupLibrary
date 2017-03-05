using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vjb.Sp.BackupLibrary.FileUtilities;
using Vjb.Sp.BackupLibrary.Model;
using Vjb.Sp.BackupLibrary.SpClient;

/*

* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
* MA 02110-1301, USA.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
* "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
* LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
* A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT
* OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
* DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
* THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
* OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
* Vidar Jon Bauge<vidarjb@gmail.com>
*/

namespace Vjb.Sp.BackupLibrary
{
    public partial class FrmMain : Form
    {
        private static BackgroundWorker bw = new BackgroundWorker();
        private List<string> files;
        
        public FrmMain()
        {
            InitializeComponent();
            files = new List<string>();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tbOutput.AppendText("Running...\r\n");
            var siteUrl = tbSiteUrl.Text;
            var listName = tbSpLibrary.Text;
            var userName = tbUserName.Text;
            var password = tbPassword.Text;

            var destinationFolder = tbTargetFolder.Text;

            bw.WorkerReportsProgress = true;
            bw.DoWork += doWork;
            var libraryInfo = new LibraryInformation(siteUrl, listName, userName, password, destinationFolder);
            bw.RunWorkerAsync(libraryInfo);
            bw.RunWorkerCompleted += bwCompleted;
        }

        private void bwCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void doWork(object sender, DoWorkEventArgs e)
        {
            var libInfo = e.Argument as LibraryInformation;

            if (libInfo == null) return;
            List<string> result = new List<string>();
            using (var sharepointClient = new SharepointClient(libInfo.SiteUrl, libInfo.UserName, libInfo.Password))
            {
                sharepointClient.OnProgressUpdate += sharepointClient_OnProgressUpdate;
                sharepointClient.OnErrorHandler += sharepointClient_ErrorHandler;
                var rootFolder = sharepointClient.GetRootFolder(libInfo.ListName);
                var folders = sharepointClient.GetFolders(rootFolder);

                sharepointClient.ProcessFolder(libInfo.DestinationFolder, rootFolder);
            }
        }

        private void sharepointClient_ErrorHandler(Exception exception)
        {
            base.Invoke((Action)delegate
            {
                tbOutput.AppendText(string.Format("The operation failed: {}\r\nStacktrace: {}", exception.Message, exception.StackTrace));
            });
        }

        private void sharepointClient_OnProgressUpdate(FileDownloadStatus status)
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
                    tbOutput.AppendText(message + "\r\n");
                }
            });
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbTargetFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbOutput.Text);
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.FileName = string.Format("FileList_{0}.csv", tbSpLibrary.Text);

            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    FileWriter.WriteToFile(files, saveFileDialog1.FileName);
                    tbOutput.AppendText(string.Format("\r\nFile list saved to {0}\r\n", saveFileDialog1.FileName));
                }
                catch (Exception exception)
                {
                    tbOutput.AppendText(string.Format("Could not save the file:{0}", exception.Message));
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAboutBox ab = new frmAboutBox();
            ab.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            using (var sharepointClient = new SharepointClient(tbSiteUrl.Text, tbUserName.Text, tbPassword.Text))
            {
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                tbSpLibrary.AutoCompleteCustomSource = acsc;
                tbSpLibrary.AutoCompleteMode = AutoCompleteMode.Suggest;
                tbSpLibrary.AutoCompleteSource = AutoCompleteSource.CustomSource;

                var libraries = sharepointClient.GetLibrarys();
                acsc.AddRange(libraries);
            }
        }
    }
}
