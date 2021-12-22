using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LeagueTierLevels.Files
{
     public class File
     {
        public static string FindFile(string extention)
        {
            string workingDir = Directory.GetCurrentDirectory();

            string[] Files = Directory.GetFiles(workingDir, "*." + extention.ToLower());

            if (Files.Length == 0)
            {
                string message = String.Format("File of type {0} could not be found", extention);
                throw new FileNotFoundException(message);
            }
            else if (Files.Length > 1)
            {
                string message = string.Format("Multiple files of type {0} found. Only one file may be present", extention);
                throw new MultipleFilesException(message);
            }
            else
            {
                return Path.Combine(workingDir, Files[0]);
            }
        }
    }
}
