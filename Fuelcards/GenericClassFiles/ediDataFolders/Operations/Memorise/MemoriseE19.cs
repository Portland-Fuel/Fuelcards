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
    public class MemoriseE19
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E19 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 34;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE19(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E19();
            Import.E19Details = new List<E19Detail>();
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
            if (_filePath.Substring(_filePath.Length - 3) != "E19") throw new ArgumentException($"The file {_filePath} is not an E19 file type, please check the file and try again.");
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

        private void ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            line = line.Replace("\"", "");
            if (line[0] == '1') ParseDetailRecord(line);
            else if (line[0] == '2') ParseControlRecord(line);
            else throw new ArgumentException($"Was expecting either 1 or 2 at the start of the line but the line is \n{line}");

        }

        private void ParseDetailRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            E19Detail d = new E19Detail();

            d.RecordType = new RecordType(p[0]);
            d.CustomerAccountCode = new Int6(p[1]);
            d.CustomerAccountSuffix = new Int2(p[2]);
            d.PanNumber = new CardNumber(p[3]);
            d.Date = new DateTime10(p[4]);
            d.Time = new TimeOnly8(p[5]);
            d.ActionStatus = new ActionStatus(p[6]);
            d.OdometerUnit = new GenericChar(p[7]);
            d.VehicleReg = new VehicleRegistration(p[8]);
            d.EmbossingDetails = new EmbossingDetails(p[9]);
            d.CardGrade = new Int2(p[10]);
            d.MileageEntryFlag = new GenericChar(p[11]);
            d.PinRequired = new GenericChar(p[12]);
            d.PinNumber = new Int4(p[13]);
            d.TelephoneRequired = new GenericChar(p[14]);
            d.ExpiryDate = new Int4(p[15]);
            d.European = new GenericChar(p[16]);
            d.Smart = new GenericChar(p[17]);
            d.SingleTransFuelLimit = new Int5(p[18]);
            d.DailyTransFuelLimit = new Int7(p[19]);
            d.WeeklyTransFuelLimit = new Int7(p[20]);
            d.NumberTransPerDay = new Int5(p[21]);
            d.NumberTransPerWeek = new Int5(p[22]);
            d.NumberFalsePinEntries = new GenericChar(p[23]);
            d.PinLockoutMinutes = new Int2(p[24]);
            d.MondayAllowed = new GenericChar(p[25]);
            d.TueasdayAllowed = new GenericChar(p[26]);
            d.WednesdayAllowed = new GenericChar(p[27]);
            d.ThursdayAllowed = new GenericChar(p[28]);
            d.FridayAllowed = new GenericChar(p[29]);
            d.SaturdayAllowed = new GenericChar(p[30]);
            d.SundayAllowed = new GenericChar(p[31]);
            d.ValidEndTime = new Int4(p[32]);
            d.ValidStartTime = new Int4(p[33]);

            Import.E19Details.Add(d);
        }

        private void ParseControlRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            Control c = new Control();

            c.RecordType = new RecordType(p[0]);
            c.CustomerCode = new Int6(p[1]);
            c.CustomerAC = new Int2(p[2]);
            c.BatchNumber = new Int9(p[3]);
            c.CreationDate = new DateTime10(p[4]);
            c.CreationTime = new TimeOnly8(p[5]);
            c.RecordCount = new Int5(p[9]);

            Import.E19Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E19Details.Count != Import.E19Control.RecordCount.Value) return false;
            return true;
        }
    }
}
