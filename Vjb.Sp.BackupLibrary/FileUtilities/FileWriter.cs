using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjb.Sp.BackupLibrary.FileUtilities
{
    public static class FileWriter
    {
        public static void WriteToFile(List<string> lines, string fileName)
        {
            File.WriteAllLines(fileName, lines.ToArray());
        }
    }
}
