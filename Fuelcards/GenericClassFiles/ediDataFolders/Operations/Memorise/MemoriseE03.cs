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
    public class MemoriseE03
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E03 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 22;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE03(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E03();
            Import.E03Details = new List<E03Detail>();
        }

        private void TestFilePath()
        {
            if (!File.Exists(_filePath)) throw new FileNotFoundException($"The file {_filePath} is not found please check the files exists and you have access to it.");
            if (_filePath.Substring(_filePath.Length - 3) != "E03") throw new ArgumentException($"The file {_filePath} is not an E03 file type, please check the file and try again.");
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
        /// <returns></returns>
        public void PutInMemory()
        {
            ParseFile();
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

        //private void ParseLine(string line)
        //{
        //    if (string.IsNullOrWhiteSpace(line)) return;
        //    line = line.Replace(",", "");
        //    line = line.Replace("\"", "");
        //    if (line[0] == '1') ParseDetailRecord(line);
        //    else if (line[0] == '2') ParseControlRecord(line);
        //    else throw new ArgumentException($"Was expecting either 1 or 2 at the start of the line but the line is \n{line}");

        //}

        //private void ParseDetailRecord(string line)
        //{
        //    if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

        //    E03Detail d = new E03Detail();

        //    // From E01Detail parsedetailrecord
        //    d.RecordType = new RecordType(line.Substring(0, 1));
        //    d.TransactionNumber = new Int9(line.Substring(1, 9));
        //    d.TransactionSequence = new Int3(line.Substring(10, 3));
        //    d.TransactionType = new TransactionType(line.Substring(13, 1));
        //    d.TransactionDate = new DateTime10(line.Substring(14, 10));
        //    d.TransactionTime = new TimeSpan8(line.Substring(24, 8));
        //    d.Period = new Int4(line.Substring(32, 4));
        //    d.SiteCode = new Int6(line.Substring(36, 6));
        //    // d.PumpNumber = new Int2(line.Substring(42, 2));
        //    d.CustomerCode = new Int6(line.Substring(42, 6));
        //    d.CustomerAC = new Int2(line.Substring(48, 2));

        //    d.CardNumber = new CardNumber(line.Substring(50, 19));
        //    d.PrimaryRegistration = new PrimaryRegistration(line.Substring(69, 12));

        //    d.Mileage = new Int7(line.Substring(81, 7));
        //    d.FleetNumber = new Int6(line.Substring(88, 6));
        //    d.ProductCode = new Int3(line.Substring(94, 3));
        //    d.Quantity = new Double9(line.Substring(97, 9));
        //    d.QuantitySign = new Sign(line.Substring(106, 1));
        //    d.Value = new Double11(line.Substring(107, 11));
        //    d.ValueSign = new Sign(line.Substring(118, 1));

        //    //d.CostSign = new Sign(line.Substring(110, 1));
        //    //d.Cost = new Double7(line.Substring(111, 7));
        //    //d.CostSign = new Sign(line.Substring(118, 1));
        //    d.AccurateMileage = new AccurateMileage(line.Substring(119, 1));
        //    d.CardRegistration = new CardRegistration(line.Substring(120, 12));
        //    d.TransactionRegistration = new VehicleRegistration(line.Substring(132, 12));

        //    Import.E03Details.Add(d);

        //}

        //private void ParseControlRecord(string line)
        //{
        //    if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

        //    Control c = new Control();

        //    c.RecordType = new RecordType(line.Substring(0, 1));
        //    c.BatchNumber = new Int9(line.Substring(1, 9));
        //    c.CreationDate = new DateTime10(line.Substring(14, 10));
        //    c.CreationTime = new TimeSpan8(line.Substring(24, 8));
        //    c.CustomerCode = new Int6(line.Substring(42, 6));
        //    c.CustomerAC = new Int2(line.Substring(48, 2));
        //    c.RecordCount = new Int7(line.Substring(81, 7));
        //    c.TotalQuantity = new Double9(line.Substring(97, 9));
        //    c.QuantitySign = new Sign(line.Substring(106, 1));
        //    c.TotalCost = new Double11(line.Substring(107, 11));
        //    c.TotalCostSign = new Sign(line.Substring(118, 1));

        //    Import.E03Control = c;

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

            E03Detail d = new E03Detail();

            d.RecordType = new RecordType(p[0]);
            d.TransactionNumber = new Int9(p[1]);
            d.TransactionSequence = new Int3(p[2]);
            d.TransactionType = new TransactionType(p[3]);
            d.TransactionDate = new DateTime10(p[4]);
            d.TransactionTime = new TimeOnly8(p[5]);
            d.Period = new Int4(p[6]);
            d.SiteCode = new Int6(p[7]);
            d.CustomerCode = new Int6(p[8]);
            d.CustomerAC = new Int2(p[9]);
            d.CardNumber = new CardNumber(p[10]);
            d.PrimaryRegistration = new PrimaryRegistration(p[11]);
            d.Mileage = new Int7(p[12]);
            d.FleetNumber = new Int6(p[13]);
            d.ProductCode = new Int3(p[14]);
            d.Quantity = new Double9(p[15]);
            d.QuantitySign = new Sign(p[16]);
            d.Value = new Double11(p[17]);
            d.ValueSign = new Sign(p[18]);
            d.AccurateMileage = new AccurateMileage(p[19]);
            d.CardRegistration = new CardRegistration(p[20]);
            d.TransactionRegistration = new VehicleRegistration(p[21]);

            Import.E03Details.Add(d);

        }

        private void ParseControlRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            Control c = new Control();

            c.RecordType = new RecordType(p[0]);
            c.BatchNumber = new Int9(p[1]);
            c.CreationDate = new DateTime10(p[4]);
            c.CreationTime = new TimeOnly8(p[5]);
            c.CustomerCode = new Int6(p[8]);
            c.CustomerAC = new Int2(p[9]);
            c.RecordCount = new Int7(p[12]);
            c.TotalQuantity = new Double11(p[15]);
            c.QuantitySign = new Sign(p[16]);
            c.TotalCost = new Double11(p[17]);
            c.TotalCostSign = new Sign(p[18]);

            Import.E03Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E03Details.Count != Import.E03Control.RecordCount.Value) return false;
            return true;
        }
    }
}
