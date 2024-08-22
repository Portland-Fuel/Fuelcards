using System;
using System.IO;
using System.Collections.Generic;
using FuelcardModels.DataTypes;
using System.Threading.Tasks;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// Takes a filepath and imports the file to memory
    /// </summary>
    public class MemoriseE23
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E23 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 39;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE23(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E23();
            Import.E23Details = new List<E23Detail>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task PutInMemoryAsync()
        {
            await ParseFileAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PutInMemory()
        {
            ParseFile();
        }

        private async Task ParseFileAsync()
        {
            int lineNumber = 1;
            using (var sr = new StreamReader(_filePath))
            {
                string line = await sr.ReadLineAsync();

                while (line != null)
                {
                    try
                    {
                        ParseLine(line);
                    }
                    catch (ArgumentNullException n)
                    {
                        throw new ArgumentNullException(GetExceptionMessage(n.Message, lineNumber));
                    }
                    catch (ArgumentOutOfRangeException o)
                    {
                        throw new ArgumentOutOfRangeException(GetExceptionMessage(o.Message, lineNumber));
                    }
                    catch (ArgumentException a)
                    {
                        throw new ArgumentException(GetExceptionMessage(a.Message, lineNumber));
                    }
                    line = sr.ReadLine();
                    lineNumber++;
                }
            }
            IsValid = ValidateImport();
        }

        private void TestFilePath()
        {
            if (!File.Exists(_filePath)) throw new FileNotFoundException($"The file {_filePath} is not found please check the files exists and you have access to it.");
            if (_filePath.Substring(_filePath.Length - 3) != "E23") throw new ArgumentException($"The file {_filePath} is not an E23 file type, please check the file and try again.");
        }

        private void ParseFile()
        {
            int lineNumber = 1;
            using (var sr = new StreamReader(_filePath))
            {
                string line = sr.ReadLine();

                while (line != null)
                {
                    try
                    {
                        ParseLine(line);
                    }
                    catch (ArgumentNullException n)
                    {
                        throw new ArgumentNullException(GetExceptionMessage(n.Message, lineNumber));
                    }
                    catch (ArgumentOutOfRangeException o)
                    {
                        throw new ArgumentOutOfRangeException(GetExceptionMessage(o.Message, lineNumber));
                    }
                    catch (ArgumentException a)
                    {
                        throw new ArgumentException(GetExceptionMessage(a.Message, lineNumber));
                    }
                    line = sr.ReadLine();
                    lineNumber++;
                }
            }
            IsValid = ValidateImport();
        }
        //Added the next three methods to account for commas in the text fields

        private void ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            line = line.Replace("\",\"", "");
            line = line.Replace("\"", "");
            if (line[0] == '1') ParseDetailRecord(line);
            else if (line[0] == '2') ParseControlRecord(line);
            else throw new ArgumentException($"Was expecting either 1 or 2 at the start of the line but the line is \n{line}");
        }

        private void ParseDetailRecord(string line)
        {
            //Directions can have commas in them which ruins the split! Get the directions out first.
            //string directions = line.Substring(289, 224).Trim();
            //line = line.Substring(0, 288) + line.Substring(513);
            //string[] p = line.Split(',');
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            if (line.Length != 522 && line.Length != 523) throw new ArgumentException($"There are too many parts to the line, there should be 522 or 523 but {line.Length} were found.");

            E23Detail d = new E23Detail();

            d.RecordType = new RecordType(line.Substring(0,1));
            d.SiteAccountCode = new Int6(line.Substring(1,6));
            d.SiteAccountSuffix = new Int2(line.Substring(7,2));
            d.SiteNewOrClosed = new GenericChar(line.Substring(9,1));
            d.Name = new Name(line.Substring(10,30));
            d.AddressLine1 = new AddressLine1(line.Substring(40,30));
            d.AddressLine2 = new AddressLine2(line.Substring(70,30));
            d.Town = new Town(line.Substring(100,30));
            d.County = new County(line.Substring(130,20));
            d.PostCode = new PostCode(line.Substring(150,10));
            d.TelephoneNumber = new TelephoneNumber(line.Substring(160,15));
            d.ContactName = new ContactName(line.Substring(175,29));
            d.RetailSite = new GenericChar(line.Substring(204,1));
            d.Canopy = new GenericChar(line.Substring(205,1));
            d.MachineType = new GenericChar(line.Substring(206,1));
            d.OpeningHours1 = new OpeningHours1(line.Substring(207,21));
            d.OpeningHours2 = new OpeningHours2(line.Substring(228,21));
            d.OpeningHours3 = new OpeningHours3(line.Substring(249,21));
            d.Directions = new Directions(line.Substring(270,225));
            d.PoleSignSupplier = new Int6(line.Substring(495,6));
            d.Parking = new GenericChar(line.Substring(501,1));
            d.Payphone = new GenericChar(line.Substring(502,1));
            d.GasOil = new GenericChar(line.Substring(503,1));
            d.Showers = new GenericChar(line.Substring(504,1));
            d.OvernnightAccomodation = new GenericChar(line.Substring(505,1));
            d.CafeRestaurant = new GenericChar(line.Substring(506,1));
            d.Toilets = new GenericChar(line.Substring(507,1));
            d.Shop = new GenericChar(line.Substring(508,1));
            d.Lubricants = new GenericChar(line.Substring(509,1));
            d.SleeperCabsWelcome = new GenericChar(line.Substring(510,1));
            d.TankCleaning = new GenericChar(line.Substring(511,1));
            d.Repairs = new GenericChar(line.Substring(512,1));
            d.WindscreenReplacement = new GenericChar(line.Substring(513,1));
            d.Bar = new GenericChar(line.Substring(514,1));
            d.CashpointMachines = new GenericChar(line.Substring(515,1));
            d.VehicleClearanceAccepted = new GenericChar(line.Substring(516,1));
            d.MotorwayJunction = new GenericChar(line.Substring(517,1));
            d.MotorwayNumber = new Int2(line.Substring(518,2));
            d.JunctionNumber = new Int2(line.Substring(520,2));

            Import.E23Details.Add(d);
        }

        private void ParseControlRecord(string line)
        {
            //string[] p = line.Split(',');
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            //if (line.Length != 207 || line.Length != 208) throw new ArgumentException($"There are too many parts to the line, there should be 207 or 208 but {line.Length} were found.");
            if (line.Length != 522 && line.Length != 523) throw new ArgumentException($"There are too many parts to the control record line, there should be 522 or 523 but {line.Length} were found.");

            Control c = new Control();

            c.RecordType = new RecordType(line.Substring(0,1));
            c.CustomerCode = new Int6(line.Substring(1,6));
            c.CustomerAC = new Int2(line.Substring(7,2));
            c.CreationDate = new DateTime10(line.Substring(10,10));
            c.CreationTime = new TimeOnly8(line.Substring(40,8));
            c.RecordCount = new Int5(line.Substring(70,5));
            c.BatchNumber = new Int9(line.Substring(100,9));
            
            Import.E23Control = c;
        }

        //private void ParseLine(string line)
        //{
        //    if (string.IsNullOrWhiteSpace(line)) return;
        //    line = line.Replace("\"", "");
        //    if (line[0] == '1') ParseDetailRecord(line);
        //    else if (line[0] == '2') ParseControlRecord(line);
        //    else throw new ArgumentException($"Was expecting either 1 or 2 at the start of the line but the line is \n{line}");

        //}

        //private void ParseDetailRecord(string line)
        //{
        //    //Directions can have commas in them which ruins the split! Get the directions out first.
        //    string directions = line.Substring(289, 224).Trim();
        //    line = line.Substring(0, 288) + line.Substring(513);
        //    string[] p = line.Split(',');
        //    if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

        //    E23Detail d = new E23Detail();

        //    d.RecordType = new RecordType(p[0]);
        //    d.SiteAccountCode = new Int6(p[1]);
        //    d.SiteAccountSuffix = new Int2(p[2]);
        //    d.SiteNewOrClosed = new GenericChar(p[3]);
        //    d.Name = new Name(p[4]);
        //    d.AddressLine1 = new AddressLine1(p[5]);
        //    d.AddressLine2 = new AddressLine2(p[6]);
        //    d.Town = new Town(p[7]);
        //    d.County = new County(p[8]);
        //    d.PostCode = new PostCode(p[9]);
        //    d.TelephoneNumber = new TelephoneNumber(p[10]);
        //    d.ContactName = new ContactName(p[11]);
        //    d.RetailSite = new GenericChar(p[12]);
        //    d.Canopy = new GenericChar(p[13]);
        //    d.MachineType = new GenericChar(p[14]);
        //    d.OpeningHours1 = new OpeningHours1(p[15]);
        //    d.OpeningHours2 = new OpeningHours2(p[16]);
        //    d.OpeningHours3 = new OpeningHours3(p[17]);
        //    d.Directions = new Directions(directions);
        //    d.PoleSignSupplier = new Int6(p[19]);
        //    d.Parking = new GenericChar(p[20]);
        //    d.Payphone = new GenericChar(p[21]);
        //    d.GasOil = new GenericChar(p[22]);
        //    d.Showers = new GenericChar(p[23]);
        //    d.OvernnightAccomodation = new GenericChar(p[24]);
        //    d.CafeRestaurant = new GenericChar(p[25]);
        //    d.Toilets = new GenericChar(p[26]);
        //    d.Shop = new GenericChar(p[27]);
        //    d.Lubricants = new GenericChar(p[28]);
        //    d.SleeperCabsWelcome = new GenericChar(p[29]);
        //    d.TankCleaning = new GenericChar(p[30]);
        //    d.Repairs = new GenericChar(p[31]);
        //    d.WindscreenReplacement = new GenericChar(p[32]);
        //    d.Bar = new GenericChar(p[33]);
        //    d.CashpointMachines = new GenericChar(p[34]);
        //    d.VehicleClearanceAccepted = new GenericChar(p[35]);
        //    d.MotorwayJunction = new GenericChar(p[36]);
        //    d.MotorwayNumber = new Int2(p[37]);
        //    d.JunctionNumber = new Int2(p[38]);

        //    Import.E23Details.Add(d);
        //}

        //private void ParseControlRecord(string line)
        //{
        //    string[] p = line.Split(',');
        //    if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

        //    Control c = new Control();

        //    c.RecordType = new RecordType(p[0]);
        //    c.CustomerCode = new Int6(p[1]);
        //    c.CustomerAC = new Int2(p[2]);
        //    c.CreationDate = new DateTime10(p[4]);
        //    c.CreationTime = new TimeSpan8(p[5]);
        //    c.RecordCount = new Int5(p[6]);
        //    if(p[7].Length > 9)
        //    {
        //        c.BatchNumber = new Int9(p[7].Substring(0,9));
        //    } else
        //    {
        //        c.BatchNumber = new Int9(p[7]);
        //    }

        //    Import.E23Control = c;
        //}

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E23Details.Count != Import.E23Control.RecordCount.Value) return false;
            return true;
        }
    }
}
