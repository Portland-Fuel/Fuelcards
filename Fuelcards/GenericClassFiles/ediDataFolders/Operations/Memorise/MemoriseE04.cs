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
    public class MemoriseE04
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E04 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 17;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE04(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E04();
            Import.E04Details = new List<E04Detail>();
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
            if (_filePath.Substring(_filePath.Length - 3) != "E04") throw new ArgumentException($"The file {_filePath} is not an E04 file type, please check the file and try again.");
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

            E04Detail d = new E04Detail();

            d.RecordType = new RecordType(p[0]);
            d.TransactionNumber = new Int9(p[1]);
            d.TransactionSequence = new Int3(p[2]);
            d.TransactionType = new TransactionType(p[3]);
            d.TransactionDate = new DateTime10(p[4]);
            d.TransactionTime = new TimeOnly8(p[5]);
            d.CustomerCode = new Int6(p[6]);
            d.CustomerAC = new Int2(p[7]);
            d.Period = new Int4(p[8]);
            d.ProductCode = new Int3(p[9]);
            d.Quantity = new Double9(p[10]);
            d.QuantitySign = new Sign(p[11]);
            d.Value = new Double11(p[12]);
            d.ValueSign = new Sign(p[13]);
            if (!string.IsNullOrWhiteSpace(p[14]))
            {
                d.CardNumber = new CardNumber(p[14]);
            }
            if (!string.IsNullOrWhiteSpace(p[15]))
            {
                d.VehicleRegistration = new VehicleRegistration(p[15]);

            }
            if (!string.IsNullOrWhiteSpace(p[16]))
            {
                d.Reference = new Reference(p[16]);
            }
            

            Import.E04Details.Add(d);

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
            c.CustomerCode = new Int6(p[6]);
            c.CustomerAC = new Int2(p[7]);
            c.TotalQuantity = new Double9(p[10]);
            c.QuantitySign = new Sign(p[11]);
            c.TotalCost = new Double11(p[12]);
            c.TotalCostSign = new Sign(p[13]);
            c.RecordCount = new Int5(p[16]);

            Import.E04Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E04Details.Count != Import.E04Control.RecordCount.Value) return false;
            return true;
        }
    }
}
