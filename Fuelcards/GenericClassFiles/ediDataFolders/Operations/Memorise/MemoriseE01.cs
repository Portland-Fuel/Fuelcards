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
    public class MemoriseE01
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E01 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 24;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE01(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E01();
            Import.E01Details = new List<E01Detail>();
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

        private void TestFilePath()
        {
            if (!File.Exists(_filePath)) throw new FileNotFoundException($"The file {_filePath} is not found please check the files exists and you have access to it.");
            if (_filePath.Substring(_filePath.Length - 3) != "E01") throw new ArgumentException($"The file {_filePath} is not an E01 file type, please check the file and try again.");
        }

        private void ParseFile()
        {
            int lineNumber = 1;
            using(var sr = new StreamReader(_filePath))
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
                    line = await sr.ReadLineAsync();
                    lineNumber++;
                }
            }
            IsValid = ValidateImport();
        }

        //private void ParseLine(string line)
        //{
        //    if (string.IsNullOrWhiteSpace(line)) return;
        //    line = line.Replace("\"", "");
        //    //line = line.Replace(",", "");
        //    if (line[0] == '1') ParseDetailRecord(line);
        //    else if (line[0] == '2') ParseControlRecord(line);
        //    else throw new ArgumentException($"Was expecting either 1 or 2 at the start of the line but the line is \n{line}");

        //}


        //private void ParseDetailRecord(string line)
        //{
        //    if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

        //    E01Detail d = new E01Detail();

        //    d.RecordType = new RecordType(line.Substring(0,1));
        //    d.TransactionNumber = new Int9(line.Substring(1,9));
        //    d.TransactionSequence = new Int3(line.Substring(10,3));
        //    d.TransactionType = new TransactionType(line.Substring(13,1));
        //    d.TransactionDate = new DateTime10(line.Substring(14,10));
        //    d.TransactionTime = new TimeSpan8(line.Substring(24,8));
        //    d.Period = new Int4(line.Substring(32,4));
        //    d.SiteCode = new Int6(line.Substring(36,6));
        //    d.PumpNumber = new Int2(line.Substring(42,2));
        //    d.CardNumber = new CardNumber(line.Substring(44,19));
        //    d.CustomerCode = new Int6(line.Substring(63,6));
        //    d.CustomerAC = new Int2(line.Substring(69,2));
        //    d.PrimaryRegistration = new VehicleRegistration(line.Substring(71,12));
        //    d.Mileage = new Int7(line.Substring(83,7));
        //    d.FleetNumber = new Int6(line.Substring(90,6));
        //    d.ProductCode = new Int3(line.Substring(96,3));
        //    d.Quantity = new Double11(line.Substring(99,11));
        //    d.CostSign = new Sign(line.Substring(110,1));
        //    d.Cost = new Double7(line.Substring(111,7));
        //    d.CostSign = new Sign(line.Substring(118,1));
        //    d.AccurateMileage = new AccurateMileage(line.Substring(129,1));
        //    d.CardRegistration = new CardRegistration(line.Substring(130,12));
        //    d.TransactionRegistration = new VehicleRegistration(line.Substring(142,12));

        //    Import.E01Details.Add(d);

        //}

        //private void ParseControlRecord(string line)
        //{
        //    if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

        //    Control c = new Control();

        //    c.RecordType = new RecordType(line.Substring(0,1));
        //    c.BatchNumber = new Int9(line.Substring(1,9));
        //    c.CreationDate = new DateTime10(line.Substring(14,10));
        //    c.CreationTime = new TimeSpan8(line.Substring(24,8));
        //    c.CustomerCode = new Int6(line.Substring(63,6));
        //    c.CustomerAC = new Int2(line.Substring(69,2));
        //    c.RecordCount = new Int7(line.Substring(83,7));
        //    c.TotalQuantity = new Double11(line.Substring(99,11));
        //    c.QuantitySign = new Sign(line.Substring(110,1));
        //    c.TotalCost = new Double7(line.Substring(111,7));
        //    c.TotalCostSign = new Sign(line.Substring(118,1));

        //    Import.E01Control = c;

        //}


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

            E01Detail d = new();

            d.RecordType = new RecordType(p[0]);
            d.TransactionNumber = new Int9(p[1]);
            d.TransactionSequence = new Int3(p[2]);
            d.TransactionType = new TransactionType(p[3]);
            d.TransactionDate = new DateTime10(p[4]);
            d.TransactionTime = new TimeOnly8(p[5]);
            d.Period = new Int4(p[6]);
            d.SiteCode = new Int6(p[7]);
            d.PumpNumber = new Int2(p[8]);
            d.CardNumber = new CardNumber(p[9]);
            d.CustomerCode = new Int6(p[10]);
            d.CustomerAC = new Int2(p[11]);
            d.PrimaryRegistration = new VehicleRegistration(p[12]);
            d.Mileage = new Int7(p[13]);
            d.FleetNumber = new Int6(p[14]);
            d.ProductCode = new Int3(p[15]);
            d.Quantity = new Double11(p[16]);
            d.CostSign = new Sign(p[17]);
            d.Cost = new Double7(p[18]);
            d.CostSign = new Sign(p[19]);
            d.AccurateMileage = new AccurateMileage(p[21]);
            d.CardRegistration = new CardRegistration(p[22]);
            d.TransactionRegistration = new VehicleRegistration(p[23]);

            Import.E01Details.Add(d);

        }

        private void ParseControlRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            Control c = new();

            c.RecordType = new RecordType(p[0]);
            c.BatchNumber = new Int9(p[1]);
            c.CreationDate = new DateTime10(p[4]);
            c.CreationTime = new TimeOnly8(p[5]);
            c.CustomerCode = new Int6(p[10]);
            c.CustomerAC = new Int2(p[11]);
            c.RecordCount = new Int7(p[13]);
            c.TotalQuantity = new Double11(p[16]);
            c.QuantitySign = new Sign(p[17]);
            c.TotalCost = new Double7(p[18]);
            c.TotalCostSign = new Sign(p[19]);

            Import.E01Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E01Details.Count != Import.E01Control.RecordCount.Value) return false;
            return true;
        }
    }
}
