using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Vjb.Sp.BackupLibrary.Model;

namespace Vjb.Sp.BackupLibrary.SpClient
{
    public class SharepointClient : IDisposable
    {
        ClientContext _clientContext;
        Folder _rootFolder;

        public string Url { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public delegate void ProgressUpdate(FileDownloadStatus status);
        public event ProgressUpdate OnProgressUpdate;
        public delegate void ErrorHandler(Exception exception);
        public event ErrorHandler OnErrorHandler;

        public SharepointClient(String url, string userName, string passWord)
        {
            try
            {
                _clientContext = new ClientContext(url);
                _clientContext.Credentials = GetCredentials(userName, passWord);
                Url = url;
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return;
            }
        }

        public string[] GetLibrarys()
        {
            IEnumerable<List> libraries;
            var result = new List<string>();

            try
            {
                var web = _clientContext.Web;
                libraries = _clientContext.LoadQuery(web.Lists.Where(l => l.BaseTemplate == 101));
                _clientContext.ExecuteQuery();
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return null;
            }

            foreach (var lib in libraries)
            {
                result.Add(lib.Title);
            }
            return result.ToArray();
        }

        public Folder GetRootFolder(string library)
        {
            var docLib = _clientContext.Web.Lists.GetByTitle(library);
            _rootFolder = docLib.RootFolder;

            try
            {
                _clientContext.Load(docLib);
                _clientContext.Load(_rootFolder);

                return _rootFolder;
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return null;
            }
        }

        public void ProcessFolder(string outPutFolder, Folder folder)
        {
            try
            {
                _clientContext.Load(folder);
                _clientContext.ExecuteQuery();

                if (folder.Name == "Forms") return;

                var srUrl = folder.ServerRelativeUrl;
                var filePath = outPutFolder;
                if (!outPutFolder.EndsWith("\\"))
                {
                    filePath += "\\";
                }

                filePath += srUrl.Replace('/', '\\') + "\\";
                Directory.CreateDirectory(filePath);

                var files = folder.Files;
                var folders = folder.Folders;

                _clientContext.Load(files);
                _clientContext.Load(folders);

                _clientContext.ExecuteQuery();
                var status = new FileDownloadStatus(FileType.Folder, folder.Name, folders.Count, files.Count);
                OnProgressUpdate(status);

                foreach (var file in folder.Files)
                {
                    ProcesFile(file, filePath);
                }

                foreach (var contentFolder in folder.Folders)
                {
                    ProcessFolder(outPutFolder, contentFolder);
                }
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return;
            }
        }

        public void ProcesFile(Microsoft.SharePoint.Client.File file, string filePath)
        {
            try
            {
                _clientContext.Load(file);
                _clientContext.ExecuteQuery();

                var fileRef = file.ServerRelativeUrl;
                var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(_clientContext, fileRef);
                var fileName = string.Format("{0}\\{1}", filePath, (string)file.Name);

                var status = new FileDownloadStatus(FileType.File, file.Name, fileName);
                OnProgressUpdate(status);

                using (var fileStream = System.IO.File.Create(fileName))
                {
                    fileInfo.Stream.CopyTo(fileStream);
                }
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return;
            }
        }

        public FolderCollection GetFolders(Folder folder)
        {
            try
            {
                var folders = folder.Folders;
                _clientContext.Load(folders);
                _clientContext.ExecuteQuery();

                return folders;
            }
            catch (Exception exception)
            {
                OnErrorHandler(exception);
                return null;
            }
        }
        private ICredentials GetCredentials(string userName, string passWord)
        {
            var securePassword = new SecureString();
            foreach(var c in passWord.ToCharArray())
            {
                securePassword.AppendChar(c);
            }
            return new SharePointOnlineCredentials(userName, securePassword);
        }

        public void Dispose()
        {
            _clientContext.Dispose();
        }
    }
}
