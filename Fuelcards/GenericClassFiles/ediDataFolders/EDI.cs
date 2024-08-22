using System;
using System.Linq;
using System.IO;
//using Portland.DataAccess.Fuelcards;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Options;
using static PortlandEmail.Email;
using System.Drawing;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;


using FuelCardModels.Operations;
using FuelcardModels.Operations;

namespace FuelcardModels.ConsoleApp
{

    class EDIs
    {
        public static void ImportAllEdi(IFuelcardUnitOfWork fuelcardRepo,string folder)
        {
            //var folder = "C:\\ExceptionUsers\\ConnorWilson\\OneDrive - Fuel Trading Company\\Desktop\\EDIs";
            NewImportE01(folder, fuelcardRepo);
            NewImportUKF(folder, fuelcardRepo);
            NewImportTex(folder, fuelcardRepo);

        }

        private static string TestFolderPath(string folder)
        {
            if (Directory.Exists(folder)) return folder;
            Console.WriteLine($"The folder ...\n\t{folder}\n...does not exist, please enter the path to the Driving Down Files Folder...");
            string response = Console.ReadLine().Replace("\"", "");
            if (Directory.Exists(response)) return response;
            return TestFolderPath(response);

        }

        static void NewImportE01(string fileOrFolder, IFuelcardUnitOfWork fuelcardRepo)
        {
            var kf = new ImportKeyFuels(fuelcardRepo);
            kf.SetListOfFilenames(fileOrFolder);
            kf.ImportKeyFuelsEDIFiles(fuelcardRepo);
            
        }
        static void NewImportUKF(string fileOrFolder, IFuelcardUnitOfWork fuelcardRepo)
        {
            var ukf = new ImportUKFuels(fuelcardRepo);
            ukf.SetListOfFilenames(fileOrFolder);
            ukf.ImportUkFuelsEDIFiles(fuelcardRepo);
        }
        static void NewImportTex(string fileOrFolder, IFuelcardUnitOfWork fuelcardRepo)
        {
            var Tex = new ImportTexaco(fuelcardRepo);
            Tex.SetListOfFilenames(fileOrFolder);
            Tex.ImportTexacoEDIFiles(fuelcardRepo);
        }
        static void ImportE01File(string fileName, IFuelcardUnitOfWork fuelcardRepo)
        {
            if (!File.Exists(fileName)) return;
            MemoriseE01 e01 = new(fileName);
            if (!e01.IsValid)
            {
                Console.WriteLine("Invalid file : " + fileName);
                return;
            }
            var dd = new GenericTransactionReport(123456);
            foreach (var e in e01.Import.E01Details)
            {
                GenericDetail d = ConvertToGenericDetail.FromKfE01Detail(e, e.CustomerCode.Value.Value);
                dd.Add(d);
                //Console.WriteLine(ConvertToGenericDetail.GenericDetailToString(d));
            }

            //Console.WriteLine("\n\nWriting the control line...\n\n");
            dd.CreateControl();
            //Console.WriteLine(dd.ControlToString());
            Console.WriteLine($"\n\nReport to string...\t{fileName}\n");
            Console.WriteLine(dd.ReportToString());
        }

        static void ImportE03File(string fileName, IFuelcardUnitOfWork fuelcardRepo)
        {
            if (!File.Exists(fileName)) return;
            MemoriseE03 e03 = new(fileName);
            if (!e03.IsValid)
            {
                Console.WriteLine("Invalid file : " + fileName);
                return;
            }
            var dd = new GenericTransactionReport(123456);
            foreach (var e in e03.Import.E03Details)
            {
                GenericDetail d = ConvertToGenericDetail.FromKfE03Detail(e, 123456);
                dd.Add(d);
                //Console.WriteLine(ConvertToGenericDetail.GenericDetailToString(d));
            }

            //Console.WriteLine("\n\nWriting the control line...\n\n");
            dd.CreateControl();
            //Console.WriteLine(dd.ControlToString());
            Console.WriteLine($"\n\nReport to string...\t{fileName}\n");
            Console.WriteLine(dd.ReportToString());
        }

        static void MoveFile(string destinationFolder, string fileName)
        {
            string folder = @"C:\Users\Steve Irwin\Documents\WFH\Driving Down\KeyFuels\E23Files";
            string source = Path.Join(folder, fileName);
            string dest = Path.Join(folder, destinationFolder, fileName);

            File.Move(source, dest, true);
        }

    }
}