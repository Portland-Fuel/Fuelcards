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
    public class MemoriseE21
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E21 Import { get; set; }

        /// <summary>
        /// IsValid gets set after the file is parsed and the validation checks have been made
        /// </summary>
        public bool IsValid { get; set; }

        private const int recordLength = 189;
        private string _filePath;

        /// <summary>
        /// Imports the data from the FilePath argument path and checks for errors.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MemoriseE21(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E21();
            Import.E21Details = new List<E21Detail>();
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
            if (_filePath.Substring(_filePath.Length - 3) != "E21") throw new ArgumentException($"The file {_filePath} is not an E21 file type, please check the file and try again.");
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

        //private void ParseDetailRecord(string line)
        //{
        //    string[] p = line.Split(',');
        //    if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

        //    E21Detail d = new E21Detail();

        //    d.RecordType = new RecordType(p[0]);
        //    d.CustomerAccountCode = new Int6(p[1]);
        //    d.CustomerAccountSuffix = new Int2(p[2]);
        //    d.Date = new DateTime10(p[3]);
        //    d.Time = new TimeSpan8(p[4]);
        //    d.ActionStatus = new ActionStatus(p[5]);
        //    d.Name = new Name(p[6]);
        //    d.AddressLine1 = new AddressLine1(p[7]);
        //    d.AddressLine2 = new AddressLine2(p[8]);
        //    d.Town = new Town(p[9]);
        //    d.County = new County(p[10]);
        //    d.PostCode = new PostCode(p[11]);

        //    Import.E21Details.Add(d);
        //}

        private void ParseDetailRecord(string line)
        {
            //string[] p = line.Split(',');
            if (line.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {line.Length} were found.");
           
            E21Detail d = new E21Detail();

            d.RecordType = new RecordType(line.Substring(0,1));
            d.CustomerAccountCode = new Int6(line.Substring(2,6));
            d.CustomerAccountSuffix = new Int2(line.Substring(9,2));
            d.Date = new DateTime10(line.Substring(12,10));
            d.Time = new TimeOnly8(line.Substring(23,8));
            d.ActionStatus = new ActionStatus(line.Substring(32,1));
            d.Name = new Name(line.Substring(34,30));
            d.AddressLine1 = new AddressLine1(line.Substring(65,30));
            d.AddressLine2 = new AddressLine2(line.Substring(96,30));
            d.Town = new Town(line.Substring(127,30));
            d.County = new County(line.Substring(158,20));
            d.PostCode = new PostCode(line.Substring(179,10));

            Import.E21Details.Add(d);
        }

        private void ParseControlRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            Control c = new Control();

            c.RecordType = new RecordType(p[0]);
            c.CustomerCode = new Int6(p[1]);
            c.CustomerAC = new Int2(p[2]);
            c.CreationDate = new DateTime10(p[3]);
            c.CreationTime = new TimeOnly8(p[4]);
            c.BatchNumber = new Int9(p[6]);
            c.RecordCount = new Int5(p[7]);

            Import.E21Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E21Details.Count != Import.E21Control.RecordCount.Value) return false;
            return true;
        }
    }
}
