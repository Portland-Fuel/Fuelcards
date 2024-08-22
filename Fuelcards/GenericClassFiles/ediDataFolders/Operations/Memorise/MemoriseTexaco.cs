using System;
using System.IO;
using System.Collections.Generic;
using FuelcardModels.DataTypes;
using FuelcardModels;
using System.Threading.Tasks;
using System.Linq;

namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoriseTexaco
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public Texaco Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        //private const int recordLength = 131;
        private const int recordLength = 108;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseTexaco(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new Texaco();
            Import.TexacoDetails = new List<TexacoDetail>();
        }

        #region Synchronous
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
        }

        /// <summary>
        /// 
        /// </summary>
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
        //Added the next three methods to account for commas in the text fields

        private void ParseLine(string line)
        {

            if (string.IsNullOrWhiteSpace(line)) return;
            string[] dataLine = line.Split(',');
            if (CrapRemover(dataLine[0].ToUpper()) == "T") { ParseControlRecord(dataLine); }
            else ParseDetailRecord(dataLine);
            //line = line.Replace("\",\"", "");
            //line = line.Replace("\"", "");
            //if (line[0] == 'T') ParseControlRecord(line);
            //else ParseDetailRecord(line);
        }

        private void ParseDetailRecord(string[] line)
        {
            if (line.Length <10 ) return;
            //Directions can have commas in them which ruins the split! Get the directions out first.
            //string directions = line.Substring(289, 224).Trim();
            //line = line.Substring(0, 288) + line.Substring(513);
            //string[] p = line.Split(',');
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            //if (line.Length != recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");

            TexacoDetail d = new();


            d.Batch = new Int3(CrapRemover(line[0]));
            d.Division = new Int3(CrapRemover(line[1]));
            d.ClientType = new GenericChar(CrapRemover(line[2]));
            d.Customer = new Long10(CrapRemover(line[3]));
            d.Site = new Long10(CrapRemover(line[4]));
            d.TranDate = new DateOnly6(CrapRemover(line[5]));
            d.TranTime = new TimeOnly4(CrapRemover(line[6]));
            d.CardNo = new Int7(CrapRemover(line[7]));
            d.Registration = new Registration(CrapRemover(line[8]));
            d.Mileage = new Int7(CrapRemover(line[10]));
            d.Quantity = new Int8(CrapRemover(line[11]));

            d.ProdNo = new Int2(CrapRemover(line[12]));
            d.MonthNo = new Int2(CrapRemover(line[16]));
            d.WeekNo = new Int1(CrapRemover(line[17]));
            d.TranNoItem = new Int9(CrapRemover(line[18]));
            d.Price = new Int8(CrapRemover(line[19]));
            d.IsoNumber = new Int6(CrapRemover(line[20]));


            Import.TexacoDetails.Add(d);
        }

        /// <summary>
        /// Creates a control record from the number of records and the total quantity and the file creation date
        /// </summary>
        /// <param name="line"></param>
        public void ParseControlRecord(string[] line)
        {
            //string[] p = line.Split(',');
            //if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");
            //if (line.Length != 207 || line.Length != 208) throw new ArgumentException($"There are too many parts to the line, there should be 207 or 208 but {line.Length} were found.");
            if (line.Length != 4) throw new ArgumentException($"The control record does not have the correct number of characters, there should be 24 but {line.Length} were found.");

            Control c = new Control();
             c.RecordType = new RecordType('T');
            c.CreationDate = new DateOnly8(CrapRemover(line[3]));
            c.CreationTime = new TimeOnly8(TimeOnly.FromDateTime(DateTime.Now));
            c.RecordCount = new Int5(CrapRemover(line[2]));
            c.TotalQuantity = new Double11(CrapRemover(line[1]));
            Import.TexacoControl = c;
            
        }


        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }
        private string CrapRemover(string line)
        {
            line = line.Replace("\",\"", "");
            line = line.Replace("\"", "");
            return line;
        }
        private bool ValidateImport()
        {
            if (Import.TexacoDetails.Count != Import.TexacoControl.RecordCount.Value) return false;
            if (Import.TexacoControl.TotalQuantity.Value != Import.TexacoDetails.Sum(d => d.Quantity.Value)) return false;
            return true;
        }

        #endregion
        #region Asynchronous


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


        #endregion
    }
}
