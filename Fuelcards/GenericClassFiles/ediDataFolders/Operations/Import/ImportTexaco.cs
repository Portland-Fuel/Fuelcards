using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using FuelcardModels.Operations;
using FuelcardModels;
using FuelCardModels.Utilities;
using FuelcardModels.DataTypes;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace FuelCardModels.Operations
{


    /// <summary>
    /// 
    /// </summary>
    public class ImportTexaco
    {
        private IQueryable<FcRequiredEdiReport> _ediAccounts;
        private IQueryable<FcNetworkAccNoToPortlandId> _accNumbers;
        private List<FileInfo> files;
        private IFuelcardUnitOfWork _db;
        private string[] fileTypes = new string[] { "fffd0742" };
        private int _controlId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public ImportTexaco(IFuelcardUnitOfWork fuelcardRepo)
        {
            //_db = db;
            _accNumbers = DbCalls.SetAccountNumbers(fuelcardRepo, Network.Texaco);
        }


        /// <summary>
        /// 
        /// </summary>
        public async Task ImportTexacoEDIFilesAsync()
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                await ImportAsync(file);
            }
        }

        #region Synchronous


        /// <summary>
        /// 
        /// </summary>
        public void ImportTexacoEDIFiles(IFuelcardUnitOfWork _db)
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                Import(file, _db);
            }
        }


        private bool ImportTexacoFile(MemoriseTexaco tex, IFuelcardUnitOfWork _db)
        {
            int network = 2;
            FcControl c = ConvertToDbControl.FileToDb(tex.Import.TexacoControl, network);
            if (DbCalls.CompareControlAgainstDb(c, _db))
            {
                _db.FcControl.Add(c);
            }
            else
            {
                return false;
            }

            _db.Save();
            _controlId = c.ControlId;
            foreach (var e in tex.Import.TexacoDetails)
            {
                TexacoTransaction u = ConvertToDbTexaco.FileToDb(e);
                u.ControlId = _controlId;
                u.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.Customer.Value.Value, _accNumbers);
                _db.TexacoTransaction.Add(u);
            }
            _db.Save();
            return true;
        }

        private MemoriseTexaco MemoriseTexaco(FileInfo file)
        {
            if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.Name} was not found to ImportE01.");
            MemoriseTexaco tex = new(file.FullName);
            tex.PutInMemory();
            if (!tex.IsValid)
            {
                Console.WriteLine("Invalid file : " + file.Name);
                throw new FileLoadException($"The file {file.Name} is not valid please check the file and try again.");
            }
            return tex;
        }


        private void CreateDrawingsEdis(FileInfo file, IFuelcardUnitOfWork _db)
        {
            if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.Texaco);
            List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
            if (introducers.Count <= 0) return;

            foreach (var intro in introducers)
            {
                string report = CreateInroducersEDI(intro, _db);
                if (string.IsNullOrWhiteSpace(report)) continue;
                FileUtils.WriteReportToFile(report, intro, file, "FF");
            }
        }



        private void Import(FileInfo file, IFuelcardUnitOfWork _db)
        {
            switch (file.Name[..8].ToLower())
            {
                case "fffd0742":
                    MemoriseTexaco tex = MemoriseTexaco(file);
                    if (ImportTexacoFile(tex, _db))
                        CreateDrawingsEdis(file,_db);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ImportAsync

        private async Task ImportAsync(FileInfo file)
        {
            switch (file.Name.Substring(0, 8).ToLower())
            {
                case "ukfd0315":
                    MemoriseTexaco tex = await MemoriseTexacoAsync(file);
                    if (await ImportTexacoAsync(tex))
                        await CreateDrawingsEdisAsync(file);
                    break;

                default:
                    break;
            }
        }

        private async Task<bool> ImportTexacoAsync(MemoriseTexaco tex)
        {
            int network = 2;
            FcControl c = ConvertToDbControl.FileToDb(tex.Import.TexacoControl, network);
            if (DbCalls.CompareControlAgainstDb(c, _db))
            {
                await _db.FcControl.AddAsync(c);
            }
            else
            {
                return false;
            }
            _db.Save();
            _controlId = c.ControlId;
            foreach (var e in tex.Import.TexacoDetails)
            {
                TexacoTransaction k = ConvertToDbTexaco.FileToDb(e);
                k.ControlId = _controlId;
                k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.Customer.Value.Value, _accNumbers);
                _db.TexacoTransaction.Add(k);
            }
            _db.Save();
            return true;
        }

        #region non drawings files

        #endregion
        #endregion

        #region Memorise file data

        private async Task<MemoriseTexaco> MemoriseTexacoAsync(FileInfo file)
        {
            if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportTexacoAsync.");
            MemoriseTexaco tex = new(file.FullName);
            await tex.PutInMemoryAsync();
            if (!tex.IsValid)
            {
                Console.WriteLine("Invalid file : " + file.FullName);
                throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
            }
            return tex;
        }

        #endregion

        #region Create EDIs

        private async Task CreateDrawingsEdisAsync(FileInfo file)
        {
            if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.Texaco);
            List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
            if (introducers.Count <= 0) return;
            IQueryable<TexacoTransaction> transactions = _db.TexacoTransaction.Where(t => t.ControlId == _controlId);
            foreach (var intro in introducers)
            {
                string report = CreateInroducersEDI(intro, _db);
                if (string.IsNullOrWhiteSpace(report)) continue;
                await FileUtils.WriteReportToFileAsync(report, intro, file, "FF");
            }
        }



        #endregion

        #region drawings file specific

        private string CreateInroducersEDI(int introducerId, IFuelcardUnitOfWork _db)
        {
            List<int> introCustomers = DbCalls.GetListOfIntroCustomersFromIntroId(introducerId, _ediAccounts);

            IQueryable<TexacoTransaction> transactions = _db.TexacoTransaction
                .Where(p => p.ControlId == _controlId &&
                introCustomers.Contains(p.PortlandId.Value));
            GenericTransactionReport dd = new(introducerId);
            foreach (TexacoTransaction t in transactions)
            {
                GenericDetail d = ConvertToGenericDetail.FromTexacoDb(t, introducerId);
                dd.Add(d);
            }
            if (dd.DrivingDownDetails.Count <= 0) return string.Empty;
            dd.CreateControl();

            return dd.ReportToString();
        }

        #endregion

        #region db calls setting parameters


        //private void SetAccountNumbers()
        //{
        //    _accNumbers = _db.Set<FcNetworkAccNoToPortlandId>().Where(f => f.Network == 0);
        //}

        /// <summary>
        /// Sets the file that are to be imported
        /// <para>Default path is '\\LS-WTGL03A\share\Fuel Card\UkFuels\Temp Files'</para>
        /// </summary>
        /// <param name="fileName">Can be either a filepath or a directory path</param>
        public void SetListOfFilenames(string fileName = "")
        {
            // Enter the default if no string is provided
            if (string.IsNullOrWhiteSpace(fileName)) fileName = new DirectoryInfo(Path.Combine(FileUtils.GetSharedFilePrefix(), @"\Fuel Card\Texaco\EDI Files Fastfuels\Temp\")).FullName;
            // check t see if the path is a directory or a file, if it is a directory then return all files in the directory
            if (Path.HasExtension(fileName))
            {
                if (fileTypes.Contains(fileName.Substring(0,8).ToLower()))
                    files = new List<FileInfo>() { new FileInfo(fileName) };
            }
            else
            {
                DirectoryInfo info = new(fileName);
                files = info.GetFiles()
                    .Where(p => fileTypes.Contains(p.Name.Substring(0,8).ToLower()))
                    .OrderBy(p => p.LastWriteTime).ToList();
            }
        }
        #endregion

        #region db calls returning values




        #endregion
    }
}
