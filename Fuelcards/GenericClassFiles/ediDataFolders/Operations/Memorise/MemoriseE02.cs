using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FuelcardModels.DataTypes;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// Takes a filepath and imports the file to memory
    /// </summary>
    public class MemoriseE02
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E02 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 18;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE02(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E02();
            Import.E02Details = new List<E02Detail>();
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
            if (_filePath.Substring(_filePath.Length - 3) != "E02") throw new ArgumentException($"The file {_filePath} is not an E02 file type, please check the file and try again.");
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

            E02Detail d = new E02Detail();

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
            d.SupplierName = new SupplierName(p[10]);
            d.ProductCode = new Int3(p[11]);
            d.Quantity = new Double11(p[12]);
            d.QuantitySign = new Sign(p[13]);
            d.CustOwnOrderNo = new CustOwnOrderNo(p[14]);
            d.CustomerOrderNo = new CustomerOrderNo(p[15]);
            d.HandlingCharge = new HandlingCharge(p[16]);
            d.DeliveryNoteNo = new DeliveryNoteNo(p[17]);

            Import.E02Details.Add(d);

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
            c.TotalQuantity = new Double11(p[12]);
            c.QuantitySign = new Sign(p[13]);
            c.RecordCount = new Int7(p[15]);

            Import.E02Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E02Details.Count != Import.E02Control.RecordCount.Value) return false;
            return true;
        }
    }
}
