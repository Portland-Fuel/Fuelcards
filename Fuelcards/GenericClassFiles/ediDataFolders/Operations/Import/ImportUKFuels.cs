using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using FuelcardModels.Operations;
using FuelcardModels;
using FuelCardModels.Utilities;
using FuelcardModels.DataTypes;
using FuelcardModels.Interfaces;
using Portland.Data.Repository.IRepository;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace FuelCardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportUKFuels
    {
        private IQueryable<FcRequiredEdiReport> _ediAccounts;
        private IQueryable<FcNetworkAccNoToPortlandId> _accNumbers;
        private IQueryable<FcMaskedCard> _maskedCards;
        private List<FileInfo> files;
        private IFuelcardUnitOfWork _db;
        private string[] fileTypes = new string[] { "ukfd0315" };
        private int _controlId;

        /// <summary>
        /// 
        /// </summary>
        public ImportUKFuels(IFuelcardUnitOfWork fuelcardRepo)
        {
            //_db = db;
            _accNumbers = DbCalls.SetAccountNumbers(fuelcardRepo, Network.UkFuels);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ImportUkFuelsEDIFilesAsync(IFuelcardUnitOfWork _db)
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                await ImportAsync(file, _db);
            }
        }

        #region Synchronous


        /// <summary>
        /// 
        /// </summary>
        public void ImportUkFuelsEDIFiles(IFuelcardUnitOfWork fuelcardRepo)
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                Import(file, fuelcardRepo);
            }
        }


        private bool ImportUkFuels(MemoriseUKFuels ukf, IFuelcardUnitOfWork _db, bool forceImport = false)
        {
            int network = 1;
            FcControl c = ConvertToDbControl.FileToDb(ukf.Import.UKFuelsControl, network);
            if (forceImport == true || DbCalls.CompareControlAgainstDb(c, _db))
            {
                _db.FcControl.Add(c);
            }
            else
            {
                return false;
            }
            _db.Save();
            _controlId = c.ControlId;
            foreach (var e in ukf.Import.UKFuelsDetails)
            {
                UkfTransaction u = ConvertToDbUkf.FileToDb(e);
                u.ControlId = _controlId;
                // TO DO - enter the Masked Calls 

                var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.Customer.Value.Value, _accNumbers);
                if (portId is not null)
                {
                    u.PortlandId = portId.Value;
                }
                else
                {
                    u.PortlandId = GetPortlandIdFromMaskedCard(e.PanNumber, _db);
                }
                _db.UkfTransactions.Add(u);
            }
            _db.Save();
            return true;
        }

        private int? GetPortlandIdFromMaskedCard(ICardNumber pan, IFuelcardUnitOfWork _db)
        {
            if (_maskedCards is null) _maskedCards = DbCalls.GetMaskedCardsForNetwork(Network.UkFuels, _db);
            var portlandId = DbCalls.GetPortlandIdFromMaskedCardNumber(pan, _maskedCards);
            return portlandId;
        }

        private MemoriseUKFuels MemoriseUKF(FileInfo file)
        {
            if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportUKFuels.");
            MemoriseUKFuels ukf = new(file.FullName);
            ukf.PutInMemory();
            if (!ukf.IsValid)
            {
                Console.WriteLine("Invalid file : " + file.FullName);
                throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
            }
            return ukf;
        }


        private void CreateDrawingsEdis(FileInfo file, IFuelcardUnitOfWork _db)
        {
            try
            {

            
            if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.UkFuels);
            List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
            if (introducers.Count <= 0) return;

            foreach (var intro in introducers)
            {
                string report = CreateIntroducersEDI(intro, _db);
                if (string.IsNullOrWhiteSpace(report)) continue;
                FileUtils.WriteReportToFile(report, intro, file, "UK");
            }
            }
            catch (Exception)
            {

                return;
            }
        }



        private void Import(FileInfo file, IFuelcardUnitOfWork fuelcardRepo)
        {
            switch (file.Name.Substring(0, 8).ToLower())
            {
                case "ukfd0315":
                    MemoriseUKFuels ukf = MemoriseUKF(file);
                    if (ImportUkFuels(ukf, fuelcardRepo))
                        CreateDrawingsEdis(file, fuelcardRepo);
                    else if (CheckWhetherToForceImport(file))
                    {
                        if (ImportUkFuels(ukf, fuelcardRepo, true))
                            CreateDrawingsEdis(file, fuelcardRepo);
                    }
                    break;
                case "copy ukf":
                    MemoriseUKFuels ukf2 = MemoriseUKF(file);
                    if (ImportUkFuels(ukf2, fuelcardRepo))
                        CreateDrawingsEdis(file, fuelcardRepo);
                    else if (CheckWhetherToForceImport(file))
                    {
                        if (ImportUkFuels(ukf2, fuelcardRepo, true))
                            CreateDrawingsEdis(file, fuelcardRepo);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates dummy pfl files that have not been imported into the Database
        /// </summary>
        public void CreateDummyPflEdi()
        {
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                if (file.Name.Substring(0, 8).ToLower() == "ukfd0315")
                {
                    MemoriseUKFuels ukf = MemoriseUKF(file);
                    // NEED TO CHECK THIS BEFORE PROCEEDING
                    int network = 1;
                    FcControl c = ConvertToDbControl.FileToDb(ukf.Import.UKFuelsControl, network);
                    int? newControl = DbCalls.GetControlIdForDummies(c, _db); // returns false if the control is in the database
                    if (newControl is null) FileUtils.CopyFileToFilesToImport(file);
                    else
                    {
                        _controlId = newControl.Value;
                        CreateDrawingsEdis(file, _db);
                    }
                }
            }
        }

        #endregion

        #region ImportAsync

        private async Task ImportAsync(FileInfo file, IFuelcardUnitOfWork _db)
        {
            switch (file.Name.Substring(0, 8).ToLower())
            {
                case "ukfd0315":
                    MemoriseUKFuels ukf = await MemoriseUKFuelsAsync(file);
                    if (ImportUkFuelsAsync(ukf))
                        await CreateDrawingsEdisAsync(file, _db);
                    break;

                default:
                    break;
            }
        }

        private bool ImportUkFuelsAsync(MemoriseUKFuels ukf)
        {
            int network = 1; 
            FcControl c = ConvertToDbControl.FileToDb(ukf.Import.UKFuelsControl, network);
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
            foreach (var e in ukf.Import.UKFuelsDetails)
            {
                UkfTransaction k = ConvertToDbUkf.FileToDb(e);
                k.ControlId = _controlId;
                k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.Customer.Value.Value, _accNumbers);
                _db.UkfTransactions.Add(k);
            }
            _db.Save();
            return true;
        }

        #region non drawings files

        #endregion
        #endregion

        #region Memorise file data

        private async Task<MemoriseUKFuels> MemoriseUKFuelsAsync(FileInfo file)
        {
            if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.Name} was not found to ImportUKFuelsAsync.");
            MemoriseUKFuels ukf = new(file.FullName);
            await ukf.PutInMemoryAsync();
            if (!ukf.IsValid)
            {
                Console.WriteLine("Invalid file : " + file.FullName);
                throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
            }
            return ukf;
        }

        #endregion

        #region Create EDIs

        private async Task CreateDrawingsEdisAsync(FileInfo file, IFuelcardUnitOfWork _db)
        {
            if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.UkFuels);
            List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
            if (introducers.Count <= 0) return;
            IQueryable<UkfTransaction> transactions = _db.UkfTransactions.Where(t => t.ControlId == _controlId);
            foreach (var intro in introducers)
            {
                string report = CreateIntroducersEDI(intro, _db);
                if (string.IsNullOrWhiteSpace(report)) continue;
                await FileUtils.WriteReportToFileAsync(report, intro, file, "UK");
            }
        }



        #endregion

        #region drawings file specific

        private string CreateIntroducersEDI(int introducerId, IFuelcardUnitOfWork _db)
        {
            List<int> introCustomers = DbCalls.GetListOfIntroCustomersFromIntroId(introducerId, _ediAccounts);

            IQueryable<UkfTransaction> transactions = _db.UkfTransactions
                .Where(p => p.ControlId == _controlId &&
                introCustomers.Contains(p.PortlandId.Value));
            GenericTransactionReport dd = new(introducerId);
            foreach (UkfTransaction t in transactions)
            {
                GenericDetail d = ConvertToGenericDetail.FromUKFuelsDb(t, introducerId);
                dd.Add(d);
            }
            if (dd.DrivingDownDetails.Count <= 0) return string.Empty;
            dd.CreateControl();

            return dd.ReportToString();
        }

        private bool CheckWhetherToForceImport(FileInfo file)
        {
            //Console.WriteLine($"\n\nThe file {file.Name} appears to be in the DB already, would you like to add this one or not (enter Y/N)?");
            //string response = Console.ReadLine();
            //if (response.ToLower() == "y") return true;
            return false;
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
            if (string.IsNullOrWhiteSpace(fileName)) fileName = new DirectoryInfo(Path.Combine(FileUtils.GetSharedFilePrefix(), @"\Fuel Card\UkFuels\Temp Files\")).FullName;
            // check t see if the path is a directory or a file, if it is a directory then return all files in the directory
            if (Path.HasExtension(fileName))
            {
                if (fileTypes.Contains(fileName.Substring(0, 8).ToLower()))
                    files = new List<FileInfo>() { new FileInfo(fileName) };
            }
            else
            {
                DirectoryInfo info = new(fileName);
                //var AllFilesAllTypes = info.GetFiles();
                files = info.GetFiles()
                    .Where(p => fileTypes.Contains(p.Name.Substring(0, 8).ToLower()))
                    .OrderBy(p => p.LastWriteTime).ToList();
                if (files.Count == 0)
                    try
                    {
                        files = info.GetFiles()
                    .Where(p => fileTypes.Contains(p.Name.Substring(5, 8).ToLower()))
                    .OrderBy(p => p.LastWriteTime).ToList();
                    }
                    catch (Exception)
                    {
                        files = info.GetFiles()
                                            .Where(p => fileTypes.Contains(p.Name.Substring(0, 8).ToLower()))
                                            .OrderBy(p => p.LastWriteTime).ToList();
                    }

            }
        }
        #endregion

        #region db calls returning values




        #endregion

    }
}
