using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FuelcardModels.DataTypes;
using FuelcardModels;
using System.Threading.Tasks;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoriseUKFuels
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public UKFuels Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 145;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseUKFuels(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new UKFuels();
            Import.UKFuelsDetails = new List<UKFuelsDetail>();
        }


        private void TestFilePath()
        {
            if (!File.Exists(_filePath)) throw new FileNotFoundException($"The file {_filePath} is not found please check the files exists and you have access to it.");
            if (_filePath.Substring(_filePath.Length - 3).ToLower() != "txt") throw new ArgumentException($"The file {_filePath} is not a valid file type, please check the file and try again.");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task PutInMemoryAsync()
        {
            await ParseFileAsync();
            ParseControlRecord(new FileInfo(_filePath).CreationTime);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PutInMemory()
        {
            ParseFile();
            ParseControlRecord(new FileInfo(_filePath).CreationTime);
        }

        #region Synchronous
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
        }
        //Added the next three methods to account for commas in the text fields

        private void ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;
            //line = line.Replace("\",\"", "");
            line = line.Replace("\"", "");
            ParseDetailRecord(line);
        }

        //private void ParseDetailRecord(string line)
        //{
        //    //Directions can have commas in them which ruins the split! Get the directions out first.
        //    //string directions = line.Substring(289, 224).Trim();
        //    //line = line.Substring(0, 288) + line.Substring(513);
        //    //string[] p = line.Split(',');
        //    //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
        //    if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

        //    UKFuelsDetail d = new UKFuelsDetail();

        //    d.Batch = new Int3(line.Substring(0, 3));
        //    d.Division = new Int3(line.Substring(3, 3));
        //    d.ClientType = new GenericChar(line.Substring(6, 1));
        //    // This should be correct according to UK Fuels docs however the customer number and site number only have 5 digits!!
        //    d.Customer = new Long10(line.Substring(7, 10));
        //    d.Site = new Long10(line.Substring(17, 10));
        //    d.TranDate = new DateTime6(line.Substring(27, 6));
        //    d.TranTime = new TimeSpan4(line.Substring(33, 4));
        //    d.CardNo = new Int7(line.Substring(37, 7));
        //    d.Registration = new Registration(line.Substring(44, 12));
        //    d.Mileage = new Int7(line.Substring(64, 7));
        //    d.Quantity = new Int8(line.Substring(71, 8));
        //    d.ProdNo = new Int2(line.Substring(79, 2));
        //    d.ReceiptNo = new ReceiptNo(line.Substring(81, 4));
        //    d.MonthNo = new Int2(line.Substring(105, 2));
        //    d.WeekNo = new Int1(line.Substring(107, 1));
        //    d.TranNoItem = new Int9(line.Substring(108, 9));
        //    d.Price = new Int8(line.Substring(117, 8));
        //    d.PanNumber = new Decimal20(line.Substring(125, 20));
            

        //    Import.UKFuelsDetails.Add(d);
        //}

        private void ParseDetailRecord(string line)
        {
            //line = line.Substring(0, 288) + line.Substring(513);
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            //if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

            UKFuelsDetail d = new UKFuelsDetail();

            d.Batch = new Int3(p[0]);
            d.Division = new Int3(p[1]);
            d.ClientType = new GenericChar(p[2]);
            // This should be correct according to UK Fuels docs however the customer number and site number only have 5 digits!!
            d.Customer = new Long10(p[3]);
            d.Site = new Long10(p[4]);
            d.TranDate = new DateOnly6(p[5]);
            d.TranTime = new TimeOnly4(p[6]);
            d.CardNo = new Int7(p[7]);
            d.Registration = new Registration(p[8]);
            d.Mileage = new Int7(p[10]);
            d.Quantity = new Int8(p[11]);
            d.ProdNo = new Int2(p[12]);
            d.ReceiptNo = new ReceiptNo(p[13]);
            d.MonthNo = new Int2(p[16]);
            d.WeekNo = new Int1(p[17]);
            d.TranNoItem = new Int9(p[18]);
            d.Price = new Int8(p[19]);
            d.PanNumber = new Decimal20(p[20]);
           

            Import.UKFuelsDetails.Add(d);
        }

        /// <summary>
        /// Creates a fake control record from the number of records and the total quantity and the file creation date
        /// </summary>
        /// <param name="fileDate"></param>
        public void ParseControlRecord(DateTime fileDate)
        {
            //string[] p = line.Split(',');
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            //if (line.Length != 207 || line.Length != 208) throw new ArgumentException($"There are too many parts to the line, there should be 207 or 208 but {line.Length} were found.");
            Control c = new Control();

            c.RecordType = new RecordType('U');
            c.CreationDate = new DateTime10(DateOnly.FromDateTime(fileDate));
            c.CreationTime = new TimeOnly8(TimeOnly.FromDateTime(fileDate));
            c.RecordCount = new Int5(Import.UKFuelsDetails.Count);
            c.TotalQuantity = new Double11(Import.UKFuelsDetails.Sum(t => (double)t.Quantity.Value));

            Import.UKFuelsControl = c;
            IsValid = true;
        }
        

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        #endregion

        private async Task ParseFileAsync()
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
                    line = await sr.ReadLineAsync();
                    lineNumber++;
                }
            }
        }
        #region Asynchronous


        #endregion
    }
}
