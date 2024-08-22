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
    public class MemoriseE06
    {
        /// <summary>
        /// Import holds the raw data once verified from the file
        /// </summary>
        public E06 Import { get; set; }

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
        public MemoriseE06(string FilePath)
        {
            _filePath = FilePath;
            TestFilePath();
            Import = new E06();
            Import.E06Details = new List<E06Detail>();
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
            if (_filePath.Substring(_filePath.Length - 3) != "E06") throw new ArgumentException($"The file {_filePath} is not an E06 file type, please check the file and try again.");
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

            E06Detail d = new E06Detail();

            d.RecordType = new RecordType(p[0]);
            d.TransactionNumber = new Int9(p[1]);
            d.TransactionSequence = new Int3(p[2]);
            d.Narrative = new Narrative(p[3]);

            Import.E06Details.Add(d);
        }

        private void ParseControlRecord(string line)
        {
            string[] p = line.Split(',');
            if (p.Length > recordLength) throw new ArgumentException($"There are too many parts to the line, there should be {recordLength} but {p.Length} were found.");

            Control c = new Control();

            c.RecordType = new RecordType(p[0]);
            c.BatchNumber = new Int9(p[1]);
            c.RecordCount = new Int3(p[2]);
            c.CustomerCode = new Int6(p[3].Substring(0,6));
            c.CustomerAC = new Int2(p[3].Substring(6,2));
            c.CreationDate = new DateOnly6(p[3].Substring(8,6));
            c.CreationTime = new TimeOnly6(p[3].Substring(14));

            Import.E06Control = c;

        }

        private string GetExceptionMessage(string message, int lineNumber)
        {
            return message + $" \nThis error occurs on line {lineNumber}.";
        }

        private bool ValidateImport()
        {
            if (Import.E06Details.Count != Import.E06Control.RecordCount.Value) return false;
            return true;
        }
    }
}
