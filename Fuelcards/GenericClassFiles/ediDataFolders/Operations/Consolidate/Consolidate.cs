using FuelcardModels;
using FuelcardModels.Operations;
using FuelCardModels.Utilities;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositorys.IRepositorys;

namespace FuelCardModels.Operations.Consolidate
{
    /// <summary>
    /// A class to consolidate the pfl files created
    /// </summary>
    public class Consolidate
    {

        private readonly string _folder;
        private readonly IFuelcardUnitOfWork fuelcardRepo;

        /// <summary>
        /// Requires the folder path as a string, where the pfl files are located
        /// </summary>
        /// <param name="folder"></param>
        //public Consolidate(string folder)
        //{
        //    _folder = folder;
        //}

        /// <summary>
        /// This is method that will consolidate the files and create the consolidated report file
        /// </summary>
        public void ConsolidateReports()
        {
            List<int> introducerIds = GetListOfIntroducersToConsolidateFilesFor(); // List of their id which is the portand_id for introducers
            if (introducerIds.Count <= 0) return;

            foreach (int id in introducerIds)
            {
                ColsolidateFiles(id);
            }
        }


        private void ColsolidateFiles(int id)
        {
            List<FileInfo> files = GetFilesToConsolidate(id);
            GenericTransactionReport report = new(id);


            foreach (FileInfo file in files)
            {
                using (var sr = new StreamReader(file.FullName))
                {
                    string line = sr.ReadLine();
                    while(line != null)
                    {
                        GenericDetail detail = ConvertToGenericDetail.StringToGenericDetail(line);
                        if (detail is not null) report.Add(detail);
                        line = sr.ReadLine();
                    }
                }
            }

            report.CreateControl();
            string reportString = report.ReportToString();
            FileInfo fileInfo = files[0];

            FileUtils.WriteReportToFile(reportString, id, fileInfo, "Consolidated");
        }




        

        

        private List<FileInfo> GetFilesToConsolidate(int id)
        {
            if(id == 0) return null;
            DirectoryInfo directoryInfo = new(_folder);
            string introducerId = id.ToString();
            
            var files = directoryInfo.EnumerateFiles().Where(f => f.Extension == ".pfl" && f.Name.Substring(0, introducerId.Length) == introducerId).ToList();

            return files;
        }

        private List<int> GetListOfIntroducersToConsolidateFilesFor()
        {
            // Just for driving down
            List<int> introducerEdiId = new();
            introducerEdiId.Add(100518);

            // To get a full list
            //return DbCalls.GetListOfAllIntroducerIds(_db);
            return introducerEdiId;
        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

    }
}
