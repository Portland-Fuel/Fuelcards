using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FuelcardModels.Operations;
using FuelcardModels;
using FuelCardModels.Utilities;
using FuelcardModels.DataTypes;
using System.Threading.Tasks;
using FuelcardModels.Interfaces;
using Portland.Data.Repository.IRepository;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.Linq.Expressions;
using DataAccess.Repositorys.IRepositorys;
using DataAccess.Fuelcards;

public class ImportKeyFuels
{
    private IQueryable<FcRequiredEdiReport> _ediAccounts;
    private readonly IQueryable<FcNetworkAccNoToPortlandId> _accNumbers;
    private IQueryable<FcMaskedCard> _maskedCards;
    private List<FileInfo> files;
    private IFuelcardUnitOfWork _db;
    private string[] fileTypes = new string[] { "e01", "e02", "e03", "e05", "e06", "e11", "e19", "e20", "e21", "e22", "e23", "e04" };
    private int _controlId;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public ImportKeyFuels(IFuelcardUnitOfWork fuelcardRepo)
    {
        //_db = db;
        _accNumbers = DbCalls.SetAccountNumbers(fuelcardRepo, Network.KeyFuels); // 0 is the for the network KeyFuels
    }

    #region Import
    /// <summary>
    /// 
    /// </summary>
    public async Task ImportKeyFuelsEDIFilesAsync()
    {
        if (files == null || files.Count == 0) return;
        foreach (var file in files)
        {
            await ImportAsync(file);
        }
    }

    #region ImportAsync

    private async Task<bool> ImportAsync(FileInfo file)
    {
        switch (file.FullName[^3..].ToLower())
        {
            case "e01":
                MemoriseE01 e01 = await MemoriseE01Async(file);
                if (ImportE01Async(e01, _db))
                    await CreateDrawingsEdisAsync(file, _db);
                break;
            case "e03":
                MemoriseE03 e03 = await MemoriseE03Async(file);
                if (ImportE03Async(e03))
                    await CreateDrawingsEdisAsync(file, _db);
                break;
            case "e02":
                MemoriseE02 e02 = await MemoriseE02Async(file);
                ImportE02Async(e02, _db);
                break;
            case "e04":
                MemoriseE04 e04 = await MemoriseE04Async(file);
                ImportE04Async(e04, _db);
                break;
            case "e05":
                //CONTACT KEYFUELS AS THE E05 FILE SEEMS TO NOT COME THROUGH IN THE FORMAT SPECIFIED
                //MemoriseE05 e05 = await MemoriseE05Async(file);
                //await ImportE05Async(e05, _db);
                break;
            case "e06":
                MemoriseE06 e06 = await MemoriseE06Async(file);
                ImportE06Async(e06, _db);
                break;
            case "e11":
                MemoriseE11 e11 = await MemoriseE11Async(file);
                await ImportE11Async(e11, _db);
                break;
            case "e19":
                MemoriseE19 e19 = await MemoriseE19Async(file);
                ImportE19Async(e19, _db);
                break;
            case "e20":
                MemoriseE20 e20 = await MemoriseE20Async(file);
                ImportE20Async(e20, _db);
                break;
            case "e21":
                MemoriseE21 e21 = await MemoriseE21Async(file);
                ImportE21Async(e21, _db);
                break;
            case "e22":
                MemoriseE22 e22 = await MemoriseE22Async(file);
                ImportE22Async(e22, _db);
                break;
            case "e23":
                MemoriseE23 e23 = await MemoriseE23Async(file);
                ImportE23Async(e23, _db);
                break;
            default:

                break;

        }
        return true;
    }

    private bool ImportE01Async(MemoriseE01 e01, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e01.Import.E01Control, network);
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
        foreach (var e in e01.Import.E01Details)
        {
            KfE1E3Transaction k = ConvertToDbE01.FileToDb(e);
            k.ControlId = _controlId;
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.CardNumber);
            }

            _db.KfE01.Add(k);
        }
        _db.Save();
        return true;
    }

    private bool ImportE03Async(MemoriseE03 e03)
    {
        int network = 0;

        FcControl c = ConvertToDbControl.FileToDb(e03.Import.E03Control, network);
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
        foreach (var e in e03.Import.E03Details)
        {
            KfE1E3Transaction k = ConvertToDbE03.FileToDb(e);
            k.ControlId = _controlId;
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.CardNumber);
            }

            _db.KfE01.Add(k);
        }
        _db.Save();
        return true;
    }

    #region non drawings files
    private bool ImportE02Async(MemoriseE02 e02, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e02.Import.E02Control, network);
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
        foreach (var e in e02.Import.E02Details)
        {
            KfE2Delivery k = ConvertToDbE02.FileToDb(e);
            if (CompareE2ToDb(k, _db))
                _db.KfE2Delivery.Add(k);
        }
        _db.Save();
        return true;
    }

    private bool ImportE04Async(MemoriseE04 e04, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e04.Import.E04Control, network);
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
        foreach (var e in e04.Import.E04Details)
        {
            KfE4SundrySale k = ConvertToDbE04.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE4SundrySales.Add(k);
        }
        _db.Save();
        return true;

    }

    private bool ImportE05Async(MemoriseE05 e05, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e05.Import.E05Control, network);
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
        foreach (var e in e05.Import.E05Details)
        {
            KfE5Stock k = ConvertToDbE05.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE5.Add(k);
        }
        _db.Save();
        return true;

    }

    private bool ImportE06Async(MemoriseE06 e06, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e06.Import.E06Control, network);
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
        foreach (var e in e06.Import.E06Details)
        {
            KfE6Transfer k = ConvertToDbE06.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE6.Add(k);
        }
        _db.Save();
        return true;

    }

    private static async Task<bool> ImportE11Async(MemoriseE11 e11, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e11.Import.E11Details)
        {
            KfE11Product k = ConvertToDbE11.FileToDb(e);
            KfE11Product c = await _db.KfE11Products.FindAsync(k.ProductCode);
            if (c == null) _db.KfE11Products.Add(k);
            else
            {
                c.ProductDescription = k.ProductDescription;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE19Async(MemoriseE19 e19, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e19.Import.E19Details)
        {
            KfE19Card k = ConvertToDbE19.FileToDb(e);
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerAccountCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.PanNumber);
            }

            KfE19Card c = (KfE19Card)_db.KfE19Cards.First(p => p.PanNumber == k.PanNumber);
            if (c == null) _db.KfE19Cards.Add(k);
            else
            {
                c.ActionStatus = k.ActionStatus;
                c.CardGrade = k.CardGrade;
                c.CustomerAccountCode = k.CustomerAccountCode;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.DailyTransFuelLimit = k.DailyTransFuelLimit;
                c.Date = k.Date;
                c.EmbossingDetails = k.EmbossingDetails;
                c.European = k.European;
                c.ExpiryDate = k.ExpiryDate;
                c.FridayAllowed = c.FridayAllowed;
                c.MileageEntryFlag = k.MileageEntryFlag;
                c.MondayAllowed = k.MondayAllowed;
                c.NumberFalsePinEntries = k.NumberFalsePinEntries;
                c.NumberTransPerDay = k.NumberTransPerDay;
                c.NumberTransPerWeek = k.NumberTransPerWeek;
                c.OdometerUnit = k.OdometerUnit;
                c.PinLockoutMinutes = k.PinLockoutMinutes;
                c.PinNumber = k.PinNumber;
                c.PinRequired = k.PinRequired;
                c.PortlandId = k.PortlandId;
                c.SaturdayAllowed = k.SaturdayAllowed;
                c.SingleTransFuelLimit = k.SingleTransFuelLimit;
                c.Smart = k.Smart;
                c.SundayAllowed = k.SundayAllowed;
                c.TelephoneRequired = k.TelephoneRequired;
                c.ThursdayAllowed = k.ThursdayAllowed;
                c.Time = k.Time;
                c.TuesdayAllowed = k.TuesdayAllowed;
                c.ValidEndTime = k.ValidEndTime;
                c.ValidStartTime = k.ValidStartTime;
                c.VehicleReg = k.VehicleReg;
                c.WednesdayAllowed = k.WednesdayAllowed;
                c.WeeklyTransFuelLimit = k.WeeklyTransFuelLimit;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE20Async(MemoriseE20 e20, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e20.Import.E20Details)
        {
            KfE20StoppedCard k = ConvertToDbE20.FileToDb(e);
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerAccountCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.PAN);
            }

            KfE20StoppedCard c = _db.KfE20.First(p => p.PanNumber == k.PanNumber);
            if (c == null) _db.KfE20.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CardId = k.CardId;
                c.CustomerAccountCode = k.CustomerAccountCode;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.StopCode = k.StopCode;
                c.Time = k.Time;
            }
        }
        _db.Save();
        return true;

    }

    private bool ImportE21Async(MemoriseE21 e21, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e21.Import.E21Details)
        {
            KfE21Account k = ConvertToDbE21.FileToDb(e);
            k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);

            KfE21Account c = _db.KfE21Accounts.Find(k.CustomerAccountCode);
            if (c == null) _db.KfE21Accounts.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.Time = k.Time;
                c.ActionStatus = k.ActionStatus;
                c.AddressLine1 = k.AddressLine1;
                c.AddressLine2 = k.AddressLine2;
                c.County = k.County;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Name = k.Name;
                c.Postcode = k.Postcode;
                c.Town = k.Town;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE22Async(MemoriseE22 e22, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e22.Import.E22Details)
        {
            KfE22AccountsStopped k = ConvertToDbE22.FileToDb(e);
            k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            KfE22AccountsStopped c = _db.KfE22AccountsStopped.First(p => p.CustomerAccountCode == k.CustomerAccountCode);
            if (c == null) _db.KfE22AccountsStopped.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.Time = k.Time;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.PersonRequestingStop = k.PersonRequestingStop;
                c.StopReferenceNumber = k.StopReferenceNumber;
                c.StopStatusCode = k.StopStatusCode;
            }
        }
        _db.Save();
        return true;

    }

    private static bool ImportE23Async(MemoriseE23 e23, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e23.Import.E23Details)
        {
            KfE23NewClosedSite k = ConvertToDbE23.FileToDb(e);
            KfE23NewClosedSite c = _db.KfE23NewClosedSites.Find(k.SiteAccountCode);
            if (c == null) _db.KfE23NewClosedSites.Add(k);
            else
            {
                c.AddressLine1 = k.AddressLine1;
                c.AddressLine2 = k.AddressLine2;
                c.County = k.County;
                c.Name = k.Name;
                c.Postcode = k.Postcode;
                c.Town = k.Town;
                c.Bar = k.Bar;
                c.CafeRestaurant = k.CafeRestaurant;
                c.Canopy = k.Canopy;
                c.CashpointMachines = k.CashpointMachines;
                c.ContactName = k.ContactName;
                c.Directions = k.Directions;
                c.Gasoil = k.Gasoil;
                c.JunctionNumber = k.JunctionNumber;
                c.Lubricants = k.Lubricants;
                c.MachineType = k.MachineType;
                c.MotorwayJunction = k.MotorwayJunction;
                c.MotorwayNumber = k.MotorwayNumber;
                c.OpeningHours1 = k.OpeningHours1;
                c.OpeningHours2 = k.OpeningHours2;
                c.OpeningHours3 = k.OpeningHours3;
                c.OvernightAccomodation = k.OvernightAccomodation;
                c.Parking = k.Parking;
                c.Payphone = k.Payphone;
                c.PoleSignSupplier = k.PoleSignSupplier;
                c.Repairs = k.Repairs;
                c.RetailSite = k.RetailSite;
                c.Shop = k.Shop;
                c.Showers = k.Showers;
                c.SiteAccountCode = k.SiteAccountCode;
                c.SiteAccountSuffix = k.SiteAccountSuffix;
                c.SiteStatus = k.SiteStatus;
                c.SleeperCabsWelcome = k.SleeperCabsWelcome;
                c.TankCleaning = k.TankCleaning;
                c.TelephoneNumber = k.TelephoneNumber;
                c.Toilets = k.Toilets;
                c.VehicleClearanceAccepted = k.VehicleClearanceAccepted;
                c.WindscreenReplacement = k.WindscreenReplacement;
            }
        }
        _db.Save();
        return true;
    }
    #endregion

    #endregion

    #region Synchronous

    /// <summary>
    /// 
    /// </summary>
    public void ImportKeyFuelsEDIFiles(IFuelcardUnitOfWork db)
    {
        if (files == null || files.Count == 0) return;
        foreach (var file in files)
        {
            Import(file, db);
        }
    }

    #region Import Synchronously
    private bool ImportE01(MemoriseE01 e01, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e01.Import.E01Control, network);
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
        foreach (var e in e01.Import.E01Details)
        {
            KfE1E3Transaction k = ConvertToDbE01.FileToDb(e);
            k.ControlId = _controlId;
            // k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerCode.Value.Value, _accNumbers);

            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.CardNumber);
            }
            _db.KfE01.Add(k);
        }
        _db.Save();
        return true;
    }

    private bool ImportE03(MemoriseE03 e03, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e03.Import.E03Control, network);
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
        foreach (var e in e03.Import.E03Details)
        {
            KfE1E3Transaction k = ConvertToDbE03.FileToDb(e);
            k.ControlId = _controlId;
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.CardNumber);
            }

            _db.KfE01.Add(k);
        }
        _db.Save();
        return true;
    }

    #region Non Drawings files
    private bool ImportE02(MemoriseE02 e02, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e02.Import.E02Control, network);
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
        foreach (var e in e02.Import.E02Details)
        {
            KfE2Delivery k = ConvertToDbE02.FileToDb(e);
            if (CompareE2ToDb(k, _db))
                _db.KfE2Delivery.Add(k);
        }
        _db.Save();
        return true;
    }

    private bool ImportE04(MemoriseE04 e04, IFuelcardUnitOfWork _db)
    {
        int network = 0;    
        FcControl c = ConvertToDbControl.FileToDb(e04.Import.E04Control, network);
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
        foreach (var e in e04.Import.E04Details)
        {
            KfE4SundrySale k = ConvertToDbE04.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE4SundrySales.Add(k);
        }
        _db.Save();
        return true;

    }

    private bool ImportE05(MemoriseE05 e05, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e05.Import.E05Control, network);
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
        foreach (var e in e05.Import.E05Details)
        {
            KfE5Stock k = ConvertToDbE05.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE5.Add(k);
        }
        _db.Save();
        return true;

    }

    private bool ImportE06(MemoriseE06 e06, IFuelcardUnitOfWork _db)
    {
        int network = 0;
        FcControl c = ConvertToDbControl.FileToDb(e06.Import.E06Control, network);
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
        foreach (var e in e06.Import.E06Details)
        {
            KfE6Transfer k = ConvertToDbE06.FileToDb(e);
            k.ControlId = _controlId;
            _db.KfE6.Add(k);
        }
        _db.Save();
        return true;

    }

    private bool ImportE11(MemoriseE11 e11, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e11.Import.E11Details)
        {
            KfE11Product k = ConvertToDbE11.FileToDb(e);
            KfE11Product c = _db.KfE11Products.Find(k.ProductCode);
            if (c == null) _db.KfE11Products.Add(k);
            else
            {
                c.ProductDescription = k.ProductDescription;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE19(MemoriseE19 e19, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e19.Import.E19Details)
        {
            KfE19Card k = ConvertToDbE19.FileToDb(e);

            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerAccountCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                portId = GetPortlandIdFromMaskedCard(e.PanNumber);
                if (portId is null) continue;
                k.PortlandId = portId.Value;
            }

            KfE19Card c = _db.KfE19Cards.GetFirstOrDefault(p => p.PanNumber == k.PanNumber);
            if (c == null) _db.KfE19Cards.Add(k);
            else
            {
                c.ActionStatus = k.ActionStatus;
                c.CardGrade = k.CardGrade;
                c.CustomerAccountCode = k.CustomerAccountCode;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.DailyTransFuelLimit = k.DailyTransFuelLimit;
                c.Date = k.Date;
                c.EmbossingDetails = k.EmbossingDetails;
                c.European = k.European;
                c.ExpiryDate = k.ExpiryDate;
                c.FridayAllowed = c.FridayAllowed;
                c.MileageEntryFlag = k.MileageEntryFlag;
                c.MondayAllowed = k.MondayAllowed;
                c.NumberFalsePinEntries = k.NumberFalsePinEntries;
                c.NumberTransPerDay = k.NumberTransPerDay;
                c.NumberTransPerWeek = k.NumberTransPerWeek;
                c.OdometerUnit = k.OdometerUnit;
                c.PinLockoutMinutes = k.PinLockoutMinutes;
                c.PinNumber = k.PinNumber;
                c.PinRequired = k.PinRequired;
                c.PortlandId = k.PortlandId;
                c.SaturdayAllowed = k.SaturdayAllowed;
                c.SingleTransFuelLimit = k.SingleTransFuelLimit;
                c.Smart = k.Smart;
                c.SundayAllowed = k.SundayAllowed;
                c.TelephoneRequired = k.TelephoneRequired;
                c.ThursdayAllowed = k.ThursdayAllowed;
                c.Time = k.Time;
                c.TuesdayAllowed = k.TuesdayAllowed;
                c.ValidEndTime = k.ValidEndTime;
                c.ValidStartTime = k.ValidStartTime;
                c.VehicleReg = k.VehicleReg;
                c.WednesdayAllowed = k.WednesdayAllowed;
                c.WeeklyTransFuelLimit = k.WeeklyTransFuelLimit;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE20(MemoriseE20 e20, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e20.Import.E20Details)
        {
            KfE20StoppedCard k = ConvertToDbE20.FileToDb(e);
            //k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            var portId = DbCalls.GetPortlandIdFromNetworkCustCode((int)e.CustomerAccountCode.Value.Value, _accNumbers);
            if (portId is not null)
            {
                k.PortlandId = portId.Value;
            }
            else
            {
                k.PortlandId = GetPortlandIdFromMaskedCard(e.PAN);
            }

            KfE20StoppedCard c = _db.KfE20.GetFirstOrDefault(p => p.PanNumber == k.PanNumber);
            if (c == null) _db.KfE20.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CardId = k.CardId;
                c.CustomerAccountCode = k.CustomerAccountCode;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.StopCode = k.StopCode;
                c.Time = k.Time;
            }
        }
        _db.Save();
        return true;

    }

    private bool ImportE21(MemoriseE21 e21, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e21.Import.E21Details)
        {
            KfE21Account k = ConvertToDbE21.FileToDb(e);
            k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            KfE21Account c = _db.KfE21Accounts.Find(k.CustomerAccountCode);
            if (c == null) _db.KfE21Accounts.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.Time = k.Time;
                c.ActionStatus = k.ActionStatus;
                c.AddressLine1 = k.AddressLine1;
                c.AddressLine2 = k.AddressLine2;
                c.County = k.County;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Name = k.Name;
                c.Postcode = k.Postcode;
                c.Town = k.Town;
            }
        }
        _db.Save();
        return true;
    }

    private bool ImportE22(MemoriseE22 e22, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e22.Import.E22Details)
        {
            KfE22AccountsStopped k = ConvertToDbE22.FileToDb(e);
            k.PortlandId = DbCalls.GetPortlandIdFromNetworkCustCode(e.CustomerAccountCode.Value.Value, _accNumbers);
            KfE22AccountsStopped c = _db.KfE22AccountsStopped.GetFirstOrDefault(p => p.CustomerAccountCode == k.CustomerAccountCode);
            if (c == null) _db.KfE22AccountsStopped.Add(k);
            else
            {
                c.PortlandId = k.PortlandId;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.Date = k.Date;
                c.Time = k.Time;
                c.CustomerAccountSuffix = k.CustomerAccountSuffix;
                c.PersonRequestingStop = k.PersonRequestingStop;
                c.StopReferenceNumber = k.StopReferenceNumber;
                c.StopStatusCode = k.StopStatusCode;
            }
        }
        _db.Save();
        return true;

    }

    private bool ImportE23(MemoriseE23 e23, IFuelcardUnitOfWork _db)
    {
        foreach (var e in e23.Import.E23Details)
        {
            KfE23NewClosedSite k = ConvertToDbE23.FileToDb(e);
            KfE23NewClosedSite c = _db.KfE23NewClosedSites.Find(k.SiteAccountCode);
            if (c == null) _db.KfE23NewClosedSites.Add(k);
            else
            {
                c.AddressLine1 = k.AddressLine1;
                c.AddressLine2 = k.AddressLine2;
                c.County = k.County;
                c.Name = k.Name;
                c.Postcode = k.Postcode;
                c.Town = k.Town;
                c.Bar = k.Bar;
                c.CafeRestaurant = k.CafeRestaurant;
                c.Canopy = k.Canopy;
                c.CashpointMachines = k.CashpointMachines;
                c.ContactName = k.ContactName;
                c.Directions = k.Directions;
                c.Gasoil = k.Gasoil;
                c.JunctionNumber = k.JunctionNumber;
                c.Lubricants = k.Lubricants;
                c.MachineType = k.MachineType;
                c.MotorwayJunction = k.MotorwayJunction;
                c.MotorwayNumber = k.MotorwayNumber;
                c.OpeningHours1 = k.OpeningHours1;
                c.OpeningHours2 = k.OpeningHours2;
                c.OpeningHours3 = k.OpeningHours3;
                c.OvernightAccomodation = k.OvernightAccomodation;
                c.Parking = k.Parking;
                c.Payphone = k.Payphone;
                c.PoleSignSupplier = k.PoleSignSupplier;
                c.Repairs = k.Repairs;
                c.RetailSite = k.RetailSite;
                c.Shop = k.Shop;
                c.Showers = k.Showers;
                c.SiteAccountCode = k.SiteAccountCode;
                c.SiteAccountSuffix = k.SiteAccountSuffix;
                c.SiteStatus = k.SiteStatus;
                c.SleeperCabsWelcome = k.SleeperCabsWelcome;
                c.TankCleaning = k.TankCleaning;
                c.TelephoneNumber = k.TelephoneNumber;
                c.Toilets = k.Toilets;
                c.VehicleClearanceAccepted = k.VehicleClearanceAccepted;
                c.WindscreenReplacement = k.WindscreenReplacement;
            }
        }
        _db.Save();
        return true;
    }
    #endregion
    #endregion

    #endregion




    private void Import(FileInfo file, IFuelcardUnitOfWork db)
    {
        switch (file.Name[^3..].ToLower())
        {
            case "e01":
                MemoriseE01 e01 = MemoriseE01(file);
                if (ImportE01(e01, db))
                    CreateDrawingsEdis(file, db);
                break;
            case "e03":
                MemoriseE03 e03 = MemoriseE03(file);
                if (ImportE03(e03, db))
                    CreateDrawingsEdis(file, db);
                break;
            case "e02":
                MemoriseE02 e02 = MemoriseE02(file);
                ImportE02(e02, db);
                break;
            case "e04":
                MemoriseE04 e04 = MemoriseE04(file);
                ImportE04(e04, db);
                break;
            case "e05":
                try
                {
                    //MemoriseE05 e05 = MemoriseE05(file);
                    //ImportE05(e05, db);
                }
                catch (FileLoadException e)
                {
                    Console.WriteLine($"Caught the exception : E05 {e.Message}\nInvalid File : {file.FullName}");
                }
                break;
            case "e06":
                MemoriseE06 e06 = MemoriseE06(file);
                ImportE06(e06, db);
                break;
            case "e11":
                MemoriseE11 e11 = MemoriseE11(file);
                ImportE11(e11, db);
                break;
            case "e19":
                MemoriseE19 e19 = MemoriseE19(file);
                ImportE19(e19, db);
                break;
            case "e20":
                MemoriseE20 e20 = MemoriseE20(file);
                ImportE20(e20, db);
                break;
            case "e21":
                try
                {
                    MemoriseE21 e21 = MemoriseE21(file);
                    ImportE21(e21, db);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Caught the exception : E21 {e.Message}\nInvalid File : {file.FullName}");
                }

                break;
            case "e22":
                MemoriseE22 e22 = MemoriseE22(file);
                ImportE22(e22, db);
                break;
            case "e23":
                MemoriseE23 e23 = MemoriseE23(file);
                ImportE23(e23, db);
                break;
            default:
                break;
        }
    }

    #endregion


    #region Create Dummy PFL EDI Files

    /// <summary>
    /// Creates dummy pfl files that have not been imported into the Database
    /// </summary>
    public void CreateDummyPflEdi()
    {
        if (files == null || files.Count == 0) return;
        foreach (var file in files)
        {
            switch (file.Name[^3..].ToLower())
            {
                case "e01":
                    int network = 0;
                    MemoriseE01 e01 = MemoriseE01(file);
                    FcControl c = ConvertToDbControl.FileToDb(e01.Import.E01Control, network);
                    int? newControl = DbCalls.GetControlIdForDummies(c, _db);
                    if (newControl is null) FileUtils.CopyFileToFilesToImport(file);
                    else
                    {
                        _controlId = newControl.Value;
                        CreateDrawingsEdis(file, _db);
                    }
                    break;
                case "e03":
                    int network2 = 0;
                    MemoriseE03 e03 = MemoriseE03(file);
                    FcControl e = ConvertToDbControl.FileToDb(e03.Import.E03Control, network2);
                    int? newEControl = DbCalls.GetControlIdForDummies(e, _db);
                    if (newEControl is null) FileUtils.CopyFileToFilesToImport(file);
                    else
                    {
                        _controlId = newEControl.Value;
                        CreateDrawingsEdis(file, _db);
                    }



                    break;
                default:
                    break;
            }
        }
    }


    #endregion

    #region Memorize

    #region Memorise file data Asynchronously

    private async Task<MemoriseE01> MemoriseE01Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE01.");
        MemoriseE01 e01 = new(file.FullName);
        await e01.PutInMemoryAsync();
        if (!e01.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e01;
    }

    private async Task<MemoriseE03> MemoriseE03Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE03.");
        MemoriseE03 e03 = new(file.FullName);
        await e03.PutInMemoryAsync();
        if (!e03.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e03;
    }


    private async Task<MemoriseE02> MemoriseE02Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE02.");
        MemoriseE02 e02 = new(file.FullName);
        await e02.PutInMemoryAsync();
        if (!e02.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e02;
    }

    private async Task<MemoriseE04> MemoriseE04Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE04.");
        MemoriseE04 e04 = new(file.FullName);
        await e04.PutInMemoryAsync();
        if (!e04.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e04;
    }

    private async Task<MemoriseE05> MemoriseE05Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE05.");
        MemoriseE05 e05 = new(file.FullName);
        await e05.PutInMemoryAsync();
        if (!e05.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e05;
    }

    private async Task<MemoriseE06> MemoriseE06Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE06.");
        MemoriseE06 e06 = new(file.FullName);
        await e06.PutInMemoryAsync();
        if (!e06.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e06;
    }

    private async Task<MemoriseE11> MemoriseE11Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe11.");
        MemoriseE11 e11 = new(file.FullName);
        await e11.PutInMemoryAsync();
        if (!e11.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e11;
    }


    private async Task<MemoriseE19> MemoriseE19Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe19.");
        MemoriseE19 e19 = new(file.FullName);
        await e19.PutInMemoryAsync();
        if (!e19.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e19;
    }


    private async Task<MemoriseE20> MemoriseE20Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe20.");
        MemoriseE20 e20 = new(file.FullName);
        await e20.PutInMemoryAsync();
        if (!e20.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e20;
    }


    private async Task<MemoriseE21> MemoriseE21Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe21.");
        MemoriseE21 e21 = new(file.FullName);
        await e21.PutInMemoryAsync();
        if (!e21.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e21;
    }

    private async Task<MemoriseE22> MemoriseE22Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe22.");
        MemoriseE22 e22 = new(file.FullName);
        await e22.PutInMemoryAsync();
        if (!e22.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e22;
    }

    private static async Task<MemoriseE23> MemoriseE23Async(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe23.");
        MemoriseE23 e23 = new(file.FullName);
        await e23.PutInMemoryAsync();
        if (!e23.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e23;
    }
    #endregion

    #region And synchronously
    private MemoriseE01 MemoriseE01(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE01.");
        MemoriseE01 e01 = new(file.FullName);
        e01.PutInMemory();
        if (!e01.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e01;
    }

    private MemoriseE03 MemoriseE03(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE03.");
        MemoriseE03 e03 = new(file.FullName);
        e03.PutInMemory();
        if (!e03.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e03;
    }


    private MemoriseE02 MemoriseE02(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE02.");
        MemoriseE02 e02 = new(file.FullName);
        e02.PutInMemory();
        if (!e02.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e02;
    }

    private MemoriseE04 MemoriseE04(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE04.");
        MemoriseE04 e04 = new(file.FullName);
        e04.PutInMemory();
        if (!e04.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e04;
    }

    private MemoriseE05 MemoriseE05(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE05.");
        MemoriseE05 e05 = new(file.FullName);
        e05.PutInMemory();
        if (!e05.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e05;
    }

    private MemoriseE06 MemoriseE06(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to ImportE06.");
        MemoriseE06 e06 = new(file.FullName);
        e06.PutInMemory();
        if (!e06.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e06;
    }

    private MemoriseE11 MemoriseE11(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe11.");
        MemoriseE11 e11 = new(file.FullName);
        e11.PutInMemory();
        if (!e11.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e11;
    }


    private MemoriseE19 MemoriseE19(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe19.");
        MemoriseE19 e19 = new(file.FullName);
        e19.PutInMemory();
        if (!e19.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e19;
    }


    private MemoriseE20 MemoriseE20(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe20.");
        MemoriseE20 e20 = new(file.FullName);
        e20.PutInMemory();
        if (!e20.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e20;
    }


    private MemoriseE21 MemoriseE21(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe21.");
        MemoriseE21 e21 = new(file.FullName);
        e21.PutInMemory();
        if (!e21.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e21;
    }

    private MemoriseE22 MemoriseE22(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe22.");
        MemoriseE22 e22 = new(file.FullName);
        e22.PutInMemory();
        if (!e22.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e22;
    }

    private MemoriseE23 MemoriseE23(FileInfo file)
    {
        if (!File.Exists(file.FullName)) throw new FileNotFoundException($"The file {file.FullName} was not found to Importe23.");
        MemoriseE23 e23 = new(file.FullName);
        e23.PutInMemory();
        if (!e23.IsValid)
        {
            Console.WriteLine("Invalid file : " + file.FullName);
            throw new FileLoadException($"The file {file.FullName} is not valid please check the file and try again.");
        }
        return e23;
    }

    #endregion

    #endregion

    #region Create EDIs

    private void CreateDrawingsEdis(FileInfo file, IFuelcardUnitOfWork _db)
    {
        if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.KeyFuels);
        List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
        if (introducers.Count <= 0) return;
        //IQueryable<KfE1E3Transaction> transactions = _db.KfE1E3Transactions.Where(t => t.ControlId == _controlId );
        foreach (var intro in introducers)
        {
            string report = CreateInroducersEDI(intro, _db);
            if (string.IsNullOrWhiteSpace(report)) continue;
            FileUtils.WriteReportToFile(report, intro, file, "KF");
        }
    }

    private async Task CreateDrawingsEdisAsync(FileInfo file, IFuelcardUnitOfWork _db)
    {
        if (_ediAccounts == null) _ediAccounts = DbCalls.SetEdiAccounts(_db, Network.KeyFuels);
        List<int> introducers = DbCalls.GetListOfIntroducers(_ediAccounts);
        if (introducers.Count <= 0) return;
        IQueryable<KfE1E3Transaction> transactions = _db.KfE01.Where(t => t.ControlId == _controlId);
        foreach (var intro in introducers)
        {
            string report = CreateInroducersEDI(intro, _db);
            if (string.IsNullOrWhiteSpace(report)) continue;
            await FileUtils.WriteReportToFileAsync(report, intro, file, "KF");
        }
    }

    #endregion

    #region drawings file specific

    private string CreateInroducersEDI(int introducerId, IFuelcardUnitOfWork _db)
    {
        // _ediAccounts comes from the fc_required_edi_reports table filtered only for the relevant network field returning true
        // _introCustomers is a list of CustomerIds that from the table where the introducer_id field matches the introducerId value
        List<int> introCustomers = DbCalls.GetListOfIntroCustomersFromIntroId(introducerId, _ediAccounts);

        IQueryable<KfE1E3Transaction> transactions = _db.KfE01
            .Where(p => p.ControlId == _controlId &&
            introCustomers.Contains(p.PortlandId.Value));
        GenericTransactionReport dd = new(introducerId);


        List<KfE1E3Transaction> listTransaction = transactions.ToList();


        foreach (KfE1E3Transaction t in transactions)
        {
            GenericDetail d = ConvertToGenericDetail.FromKfE01E03Db(t, introducerId);
            dd.Add(d);
        }
        if (dd.DrivingDownDetails.Count <= 0) return string.Empty;
        dd.CreateControl();

        return dd.ReportToString();
    }

    #endregion

    #region db calls setting parameters

    private int? GetPortlandIdFromMaskedCard(ICardNumber pan)
    {
        if (_maskedCards is null) _maskedCards = DbCalls.GetMaskedCardsForNetwork(Network.KeyFuels, _db);
        var portlandId = DbCalls.GetPortlandIdFromMaskedCardNumber(pan, _maskedCards);
        return portlandId;
    }

    //private void SetAccountNumbers()
    //{
    //    _accNumbers = _db.Set<FcNetworkAccNoToPortlandId>().Where(f => f.Network == 0);
    //}

    /// <summary>
    /// Sets the file that are to be imported
    /// <para>Default path is '\\LS-WTGL03A\share\Fuel Card\Keyfuels\Temp Files'</para>
    /// </summary>
    /// <param name="fileName">Can be either a filepath or a directory path</param>
    public void SetListOfFilenames(string fileName = "")
    {
        // Enter the default if no string is provided
        if (string.IsNullOrWhiteSpace(fileName)) fileName = new DirectoryInfo(Path.Combine(FileUtils.GetSharedFilePrefix(), @"\Fuel Card\Keyfuels\Temp Files\")).FullName;
        // check t see if the path is a directory or a file, if it is a directory then return all files in the directory
        if (Path.HasExtension(fileName))
        {
            if (fileTypes.Contains(fileName.Substring(fileName.Length - 3).ToLower()))
                files = new List<FileInfo>() { new FileInfo(fileName) };
        }
        else
        {
            DirectoryInfo info = new(fileName);

            files = info.GetFiles()
                .Where(p => fileTypes.Contains(p.Name.Substring(p.Name.Length - 3).ToLower()))
                .OrderBy(p => p.LastWriteTime).ToList();
        }
    }
    #endregion

    #region Compare file to DB (only where relevant)

    /// <summary>
    /// Checks to see if the E02 record is already in the db
    /// <para>If it is already in this returns false, if it is not in and</para>
    /// <para>needs adding it returns true</para>
    /// </summary>
    /// <param name="k"></param>
    /// <returns>true if the control needs adding</returns>
    /// <returns>false if it already in the database</returns>
    private bool CompareE2ToDb(KfE2Delivery k, IFuelcardUnitOfWork _db)
    {
        KfE2Delivery d = _db.KfE2Delivery.GetFirstOrDefault(d =>
            d.ControlId == k.ControlId &&
            d.CustomerCode == k.CustomerCode &&
            d.TransactionNumber == k.TransactionNumber &&
            d.TransactionDate == k.TransactionDate &&
            d.TransactionTime == k.TransactionTime &&
            d.TransactionType == k.TransactionType &&
            d.SiteCode == k.SiteCode &&
            d.ProductCode == k.ProductCode &&
            d.Quantity == k.Quantity &&
            d.QuantitySign == k.QuantitySign &&
            d.HandlingCharge == k.HandlingCharge
        );
        return (d == null);
    }

    /// <summary>
    /// Checks to see if the E04 record is already in the db
    /// <para>If it is already in this returns false, if it is not in and</para>
    /// <para>needs adding it returns true</para>
    /// </summary>
    /// <param name="k"></param>
    /// <returns>true if the control needs adding</returns>
    /// <returns>false if it already in the database</returns>
    private bool CompareE4ToDb(KfE4SundrySale k)
    {
        KfE4SundrySale d = _db.KfE4SundrySales.First(d =>
            d.ControlId == k.ControlId &&
            d.CustomerCode == k.CustomerCode &&
            d.TransactionNumber == k.TransactionNumber &&
            d.TransactionDate == k.TransactionDate &&
            d.TransactionTime == k.TransactionTime &&
            d.TransactionType == k.TransactionType &&
            d.ProductCode == k.ProductCode &&
            d.Quantity == k.Quantity &&
            d.QuantitySign == k.QuantitySign
        );
        return (d == null);
    }

    /// <summary>
    /// Checks to see if the E05 record is already in the db
    /// <para>If it is already in this returns false, if it is not in and</para>
    /// <para>needs adding it returns true</para>
    /// </summary>
    /// <param name="k"></param>
    /// <returns>true if the control needs adding</returns>
    /// <returns>false if it already in the database</returns>
    private bool CompareE5ToDb(KfE5Stock k)
    {
        KfE5Stock d = _db.KfE5.First(d =>
            d.ControlId == k.ControlId &&
            d.CustomerCode == k.CustomerCode &&
            d.ProductCode == k.ProductCode &&
            d.OpeningStockBalance == k.OpeningStockBalance &&
            d.ClosingStockBalance == k.ClosingStockBalance &&
            d.DeliveryQuantity == k.DeliveryQuantity &&
            d.DrawingQuantity == k.DrawingQuantity
        );
        return (d == null);
    }

    /// <summary>
    /// Checks to see if the E06 record is already in the db
    /// <para>If it is already in this returns false, if it is not in and</para>
    /// <para>needs adding it returns true</para>
    /// </summary>
    /// <param name="k"></param>
    /// <returns>true if the control needs adding</returns>
    /// <returns>false if it already in the database</returns>
    private bool CompareE6ToDb(KfE6Transfer k)
    {
        KfE6Transfer d = _db.KfE6.First(d =>
            d.ControlId == k.ControlId &&
            d.TransactionNumber == k.TransactionNumber &&
            d.Id == k.Id
        );
        return (d == null);
    }


    #endregion

}
