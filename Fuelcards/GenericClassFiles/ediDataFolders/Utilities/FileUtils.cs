using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortlandCredentials;

namespace FuelCardModels.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUtils
    {
        #region file related methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="introducersId"></param>
        /// <param name="file"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static async Task WriteReportToFileAsync(string report, int introducersId, FileInfo file, string detail = "")
        {
            string fileName = GetFullFilenameForEdi(introducersId, file, detail, "");
            using StreamWriter sw = new StreamWriter(fileName);
            await sw.WriteAsync(report);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="introducersId"></param>
        /// <param name="file"></param>
        /// <param name="detail"></param>
        /// <param name="defaultdirectory"></param>
        /// <returns>Filename in the format yyMMdd hhmmssff</returns>
        public static string GetFullFilenameForEdi(int introducersId, FileInfo file, string detail = "", string defaultdirectory = "")
        {
            string fileName = GetDirectoryName(file, defaultdirectory);
            fileName += @"\" + introducersId.ToString() + " ";
            if (!string.IsNullOrWhiteSpace(detail)) fileName += detail + " ";
            fileName += DateTime.Now.ToString("yyMMddhhmmssfffff") + ".pfl";
            return fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="defaultdirectory"></param>
        /// <returns></returns>
        public static string GetDirectoryName(FileInfo file, string defaultdirectory = "")
        {
            if (string.IsNullOrWhiteSpace(defaultdirectory)) return file.DirectoryName;
            if (defaultdirectory.Substring(defaultdirectory.Length - 1) == @"\") defaultdirectory = defaultdirectory.Substring(0, defaultdirectory.Length - 1);
            if (Directory.Exists(defaultdirectory)) return defaultdirectory;
            return file.DirectoryName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="introducersId"></param>
        /// <param name="file"></param>
        /// <param name="detail"></param>
        public static void WriteReportToFile(string report, int introducersId, FileInfo file, string detail = "")
        {
            string fileName = FileUtils.GetFullFilenameForEdi(introducersId, file, detail, "");
            using StreamWriter sw = new StreamWriter(fileName);
            sw.Write(report);
        }

        /// <summary>
        /// Used when creating dummy files, if not imported copies the file to a new directory ready to be imported later.
        /// </summary>
        /// <param name="file"></param>
        public static void CopyFileToFilesToImport(FileInfo file)
        {

            Console.WriteLine($"Looks like this file {file.Name} is not imported I will put it in a seperate folder for you.");
            var folder = Directory.CreateDirectory(Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "DrivingDownFilesFolder", "FilesToBeImported"));
            var destFile = Path.Combine(folder.FullName, file.Name);

            if(!File.Exists(file.FullName)) File.Copy(file.FullName, destFile);
        }

        public static string GetSharedFilePrefix()
        {
            //return Credentials.GetPortlandFileShareSource().FileShareSource;
            return "";
        }

        #endregion
    }
}
