using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjb.Sp.BackupLibrary.Model
{
    public class FileDownloadStatus
    {
        public FileType FileType { get; private set; }
        public string FileName { get; private set; }
        public int FolderCount { get; private set; }
        public int FilesCount { get; private set; }
        public string Destination { get; private set; }


        public FileDownloadStatus(FileType type, string name, int count, int filesCount)
        {
            FileType = type;
            FileName = name;
            FolderCount = count;
            FilesCount = filesCount;
        }

        public FileDownloadStatus(FileType type, string name, string destination)
        {
            FileType = type;
            FileName = name;
            Destination = destination;
        }
    }

    public enum FileType {
        Folder,
        File
   }
}
