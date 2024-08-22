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
    public class MemoriseE05
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E05 Import { get; set; }

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
        public MemoriseE05(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E05();
            Import.E05Details = new List<E05Detail>();
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
            try
            {
                ParseFile();
            }catch(ArgumentException e)
            {
                Console.WriteLine($"Invalid file : {_filePath}\nError Message : {e.Message}");
            }
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
            if (_filePath.Substring(_filePath.Length - 3) != "E05") throw new ArgumentException($"The file {_filePath} is not an E05 file type, please check the file and try again.");
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

            E05Detail d = new E05Detail();

            d.RecordType = new RecordType(p[0]);
            d.CustomerCode = new Int6(p[1]);
            d.CustomerAC = new Int2(p[2]);
            d.ProductCode = new Int3(p[3]);
            d.OpeningStockBalance = new Double11(p[4]);
            d.OpeningBalanceSign = new Sign(p[5]);
            d.DrawingQuantity = new Double11(p[6]);
            d.DrawingQuantitySign = new Sign(p[7]);
            d.NumberOfDrawings = new Int7(p[8]);
            d.DeliveryQuantity = new Double11(p[9]);
            d.DeliveryQuantitySign = new Sign(p[10]);
            d.NumberOfDeliveries = new Int5(p[11]);
            d.ClosingStockBalance = new Double11(p[12]);
            d.ClosingBalanceSign = new Sign(p[13]);
            d.Period = new Int4(p[14]);
            
            Import.E05Details.Add(d);
        }

        //private void ParseControlRecord(string line)
        //{
        //    string[] p = line.Split(',');
        //    if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

        //    Control c = new Control();

        //    c.RecordType = new RecordType(p[0]);
        //    c.CustomerCode = new Int6(p[1]);
        //    c.CustomerAC = new Int2(p[2]); 
        //    c.BatchNumber = new Int9(p[4]);
        //    c.RecordCount = new Int5(p[11]);
        //    c.CreationDate = new DateTime6(p[15]);
        //    c.CreationTime = new TimeSpan6(p[16]);
            

        //    Import.E05Control = c;

        //}

        private void ParseControlRecord(string line)
        {
            string p = line.Replace(",", "");
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
          Control c = new Control();

            c.RecordType = new RecordType(p.Substring(0,1));
            c.CustomerCode = new Int6(p.Substring(1, 6));
            c.CustomerAC = new Int2(p.Substring(7, 2));
            c.BatchNumber = new Int9(p.Substring(14, 5));
            c.RecordCount = new Int5(p.Substring(55, 5));
            c.CreationDate = new DateOnly6(p.Substring(76, 6));
            c.CreationTime = new TimeOnly6(p.Substring(82, 6));


            Import.E05Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E05Details.Count != Import.E05Control.RecordCount.Value) return false;
            return true;
        }
    }
}
