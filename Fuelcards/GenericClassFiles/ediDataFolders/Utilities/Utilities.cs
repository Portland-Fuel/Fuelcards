using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelcardModels.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns an Int
        /// </summary>
        /// <param name="IntAsString"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GetIntFromString(string IntAsString, int NoOfDigits = 7)
        {
            if (string.IsNullOrWhiteSpace(IntAsString)) throw new ArgumentNullException($"The string {IntAsString} cannot be null, whitespace or empty.");
            string trimmed = IntAsString.Trim();
            if (int.TryParse(trimmed, out int result))
            {
                return CheckIntForSize(result, NoOfDigits);
            }
            else
            {
                throw new ArgumentException($"Cannot convert {IntAsString} to an int.");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns an Int
        /// </summary>
        /// <param name="LongAsString"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static long GetLongFromString(string LongAsString, int NoOfDigits = 10)
        {
            if (string.IsNullOrWhiteSpace(LongAsString)) throw new ArgumentNullException($"The string {LongAsString} cannot be null, whitespace or empty.");
            string trimmed = LongAsString.Trim();
            if (long.TryParse(trimmed, out long result))
            {
                return CheckLongForSize(result, NoOfDigits);
            }
            else
            {
                throw new ArgumentException($"Cannot convert {LongAsString} to a long.");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a 2 digit (max) Sbyte
        /// </summary>
        /// <param name="SbyteAsString"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static sbyte GetSbyteFromString(string SbyteAsString, int NoOfDigits = 2)
        {
            if (string.IsNullOrWhiteSpace(SbyteAsString)) throw new ArgumentNullException($"The string {SbyteAsString} cannot be null, whitespace or empty.");
            string trimmed = SbyteAsString.Trim();
            if (sbyte.TryParse(trimmed, out sbyte result))
            {
                if (result > -Math.Pow(10, NoOfDigits) && result < Math.Pow(10, NoOfDigits)) return result;
                throw new ArgumentOutOfRangeException($"The sbyte is out of range, it should be between -100 and 100 but it is {result}");
            }
            else
            {
                throw new ArgumentException($"Cannot convert {SbyteAsString} to an int.");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a Short
        /// </summary>
        /// <param name="ShortAsString"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static short GetShortFromString(string ShortAsString, int NoOfDigits = 2)
        {
            if (string.IsNullOrWhiteSpace(ShortAsString)) throw new ArgumentNullException($"The string {ShortAsString} cannot be null, whitespace or empty.");
            string trimmed = ShortAsString.Trim();
            if (short.TryParse(trimmed, out short result))
            {
                if (result > -Math.Pow(10, NoOfDigits) && result < Math.Pow(10, NoOfDigits)) return result;
                throw new ArgumentOutOfRangeException($"The short is out of range, it should be between -1,000 and 1,000 but it is {result}");
            }
            else
            {
                throw new ArgumentException($"Cannot convert {ShortAsString} to an int.");
            }
        }

        /// <summary>
        /// Checks the size if the int and returns it if OK
        /// </summary>
        /// <param name="IntToCheck"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int CheckIntForSize(int IntToCheck, int NoOfDigits)
        {
            if (IntToCheck >= Math.Pow(10, NoOfDigits)) throw new ArgumentOutOfRangeException($"The int {IntToCheck} should only have {NoOfDigits} digits.");
            return IntToCheck;
        }


        /// <summary>
        /// Checks the size if the int and returns it if OK
        /// </summary>
        /// <param name="LongToCheck"></param>
        /// <param name="NoOfDigits"></param>
        /// <returns>int</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static long CheckLongForSize(long LongToCheck, int NoOfDigits)
        {
            if (LongToCheck >= Math.Pow(10, NoOfDigits)) throw new ArgumentOutOfRangeException($"The long {LongToCheck} should only have {NoOfDigits} digits.");
            return LongToCheck;
        }


        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a Char
        /// <para>If the string contains more than one char the forst will be returned</para>
        /// </summary>
        /// <param name="CharAsString"></param>
        /// <returns>char</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static char? GetCharFromString(string CharAsString)
        {
            if (string.IsNullOrWhiteSpace(CharAsString)) return null;
            string trimmed = CharAsString.Trim();
            return trimmed[0];
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a long
        /// </summary>
        /// <param name="LongAsString"></param>
        /// <returns>long</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static long GetLongFromString(string LongAsString)
        {
            if (string.IsNullOrWhiteSpace(LongAsString)) throw new ArgumentNullException($"The string {LongAsString} cannot be null, whitespace or empty.");
            string trimmed = LongAsString.Trim();
            if (long.TryParse(trimmed, out long result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Cannot convert {LongAsString} to a long.");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a ulong
        /// </summary>
        /// <param name="UlongAsString"></param>
        /// <returns>ulong</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static ulong GetUlongFromString(string UlongAsString)
        {
            if (string.IsNullOrWhiteSpace(UlongAsString)) throw new ArgumentNullException($"The string {UlongAsString} cannot be null, whitespace or empty.");
            string trimmed = UlongAsString.Trim();
            if (ulong.TryParse(trimmed, out ulong result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Cannot convert {UlongAsString} to a ulong.");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a double
        /// </summary>
        /// <param name="DoubleAsString"></param>
        /// <param name="Len"></param>
        /// <param name="DecPlaces"></param>
        /// <returns>double</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetDoubleFromString(string DoubleAsString, int Len = 0, int DecPlaces = -1)
        {
            if (string.IsNullOrWhiteSpace(DoubleAsString)) throw new ArgumentNullException($"The string {DoubleAsString} cannot be null, whitespace or empty.");
            string trimmed = DoubleAsString.Trim();
            if (double.TryParse(trimmed, out double result))
            {
                return CheckDoubleDimensions(result, Len, DecPlaces);
            }
            else
            {
                throw new ArgumentException($"Cannot convert {DoubleAsString} to a double.");
            }
        }
        /// <summary>
        /// Takes three parameters and checks the doubles is within spec
        /// </summary>
        /// <param name="Number">The value as a double data type</param>
        /// <param name="Len">int the total length of the number</param>
        /// <param name="DecPlaces">the number of decimal places the number needs to be accurate to</param>
        /// <returns>double</returns>
        /// <exception cref="ArgumentException"></exception>
        public static double CheckDoubleDimensions(double Number, int Len, int DecPlaces = -1)
        {
            //Check for the size of the number
            if (DecPlaces != -1) Len = Len - DecPlaces;
            if (Len > 0)
            {
                double maxOrMin = Math.Pow(10, Len);
                if (Number >= maxOrMin || Number <= -maxOrMin) throw new ArgumentException($"The number is out of range, it should be within ±{maxOrMin} and it is {Number}");
            }
            //Check for rounding and trim to required decimal places
            if (DecPlaces != -1) Number = Math.Round(Number, DecPlaces, MidpointRounding.AwayFromZero);
            return Number;
        }


        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a decimal
        /// </summary>
        /// <param name="DecimalAsString"></param>
        /// <param name="Len"></param>
        /// <param name="DecPlaces"></param>
        /// <returns>double</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal GetDecimalFromString(string DecimalAsString, int Len = 0, int DecPlaces = -1)
        {
            if (string.IsNullOrWhiteSpace(DecimalAsString)) throw new ArgumentNullException($"The string {DecimalAsString} cannot be null, whitespace or empty.");
            string trimmed = DecimalAsString.Trim();
            if (decimal.TryParse(trimmed, out decimal result))
            {
                return CheckDecimalDimensions(result, Len, DecPlaces);
            }
            else
            {
                throw new ArgumentException($"Cannot convert {DecimalAsString} to a decimal.");
            }
        }

        /// <summary>
        /// Takes three parameters and checks the decimal is within spec
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Len">The total length of the number</param>
        /// <param name="DecPlaces">The number of decimal places the number should be rounded to</param>
        /// <returns>decimal</returns>
        /// <exception cref="ArgumentException"></exception>
        public static decimal CheckDecimalDimensions(decimal Number, int Len, int DecPlaces = -1)
        {
            //Check for the size of the number
            if (DecPlaces != -1) Len = Len - DecPlaces;
            if (Len > 0)
            {
                decimal maxOrMin = (decimal)Math.Pow(10, Len);
                if (Number >= maxOrMin || Number <= -maxOrMin) throw new ArgumentException($"The number is out of range, it should be within ±{maxOrMin} and it is {Number}");
            }
            //Check for rounding and trim to required decimal places
            if (DecPlaces != -1) Number = Math.Round(Number, DecPlaces, MidpointRounding.AwayFromZero);
            return Number;
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a DateTime
        /// <para>The string must be in the format 'yyyy-MM-dd'</para>
        /// </summary>
        /// <param name="DateTimeAsString"></param>
        /// <returns>DateTime</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static DateOnly? GetDateOnlyFromString(string DateTimeAsString)
        {
            if (string.IsNullOrWhiteSpace(DateTimeAsString)) throw new ArgumentNullException($"The string {DateTimeAsString} cannot be null, whitespace or empty.");
            string trimmed = DateTimeAsString.Trim();
            if (DateTime.TryParseExact(trimmed, "yyyy-MM-dd", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime result))
            {
                DateOnly date = DateOnly.FromDateTime(result);
                return date;
            }
            else
            {
                throw new ArgumentException($"Cannot convert the string {DateTimeAsString} to DateOnly, it is not in the format yyyy-MM-ss");

            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a DateTime
        /// <para>The string must be in the format 'yyyy-MM-dd'</para>
        /// </summary>
        /// <param name="DateTimeAs8String"></param>
        /// <returns>DateTime</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static DateOnly? GetDateOnlyFrom8String(string DateTimeAs8String)
        {
            if (string.IsNullOrWhiteSpace(DateTimeAs8String)) throw new ArgumentNullException($"The string {DateTimeAs8String} cannot be null, whitespace or empty.");
            string trimmed = DateTimeAs8String.Trim();
            if (DateTime.TryParseExact(trimmed, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime result))
            {
                DateOnly date = DateOnly.FromDateTime(result);
                return date;
            }
            else
            {
                throw new ArgumentException($"Cannot convert the string {DateTimeAs8String} to DateOnly, it is not in the format yyyyMMdd");

            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a DateTime
        /// <para>The string must be in the format 'yyyy-MM-dd'</para>
        /// </summary>
        /// <param name="DateTimeAs6String"></param>
        /// <returns>DateTime</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static DateOnly? GetDateOnlyFrom6String(string DateTimeAs6String)
        {
            if (string.IsNullOrWhiteSpace(DateTimeAs6String)) throw new ArgumentNullException($"The string {DateTimeAs6String} cannot be null, whitespace or empty.");
            string trimmed = DateTimeAs6String.Trim();
            if (DateTime.TryParseExact(trimmed, "ddMMyy", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime result))
            {
                DateOnly date = DateOnly.FromDateTime(result);
                return date;
            }
            else
            {
                throw new ArgumentException($"Cannot convert the string {DateTimeAs6String} to DateTime, it is not in the format ddMMyy");

            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a TimeSpan
        /// </summary>
        /// <param name="TimeSpanAsString"></param>
        /// <returns>TimeSpan</returns>
        /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static TimeOnly? GetTimeOnlyFromString(string TimeSpanAsString)
        {
            if (string.IsNullOrWhiteSpace(TimeSpanAsString)) throw new ArgumentNullException($"The string {TimeSpanAsString} cannot be null, whitespace or empty.");
            string trimmed = TimeSpanAsString.Trim();
            if (TimeSpan.TryParseExact(trimmed, "hh\\.mm\\.ss", new CultureInfo("en-GB"), TimeSpanStyles.None, out TimeSpan result))
            {
                TimeOnly time = TimeOnly.FromTimeSpan(result);
                return time;
            }
            else
            {
                if (TimeSpan.TryParseExact(trimmed, "hh\\:mm\\:ss", new CultureInfo("en-GB"), TimeSpanStyles.None, out TimeSpan secondResult))
                {
                    TimeOnly time = TimeOnly.FromTimeSpan(secondResult);
                    return time;
                }
                throw new ArgumentException($"Cannot convert string {TimeSpanAsString} to TimeOnly, it is not in the format HH.mm.ss");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a TimeSpan
        /// </summary>
        /// <param name="TimeSpanAs6String"></param>
        /// <returns>TimeSpan</returns>
        /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static TimeOnly? GetTimeOnlyFrom6String(string TimeSpanAs6String)
        {
            if (string.IsNullOrWhiteSpace(TimeSpanAs6String)) throw new ArgumentNullException($"The string {TimeSpanAs6String} cannot be null, whitespace or empty.");
            string trimmed = TimeSpanAs6String.Trim();
            if (TimeSpan.TryParseExact(trimmed, "hhmmss", new CultureInfo("en-GB"), out TimeSpan result))
            {
                TimeOnly time = TimeOnly.FromTimeSpan(result);
                return time;
            }
            else
            {
                throw new ArgumentException($"Cannot convert string {TimeSpanAs6String} to TimeOnly, it is not in the format HHmmss");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace and returns a TimeSpan
        /// </summary>
        /// <param name="TimeSpanAs4String"></param>
        /// <returns>TimeSpan</returns>
        /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static TimeOnly? GetTimeOnlyFrom4String(string TimeSpanAs4String)
        {
            if (string.IsNullOrWhiteSpace(TimeSpanAs4String)) throw new ArgumentNullException($"The string {TimeSpanAs4String} cannot be null, whitespace or empty.");
            string trimmed = TimeSpanAs4String.Trim();
            if (TimeSpan.TryParseExact(trimmed, "hhmm", new CultureInfo("en-GB"), out TimeSpan result))
            {
                TimeOnly time = TimeOnly.FromTimeSpan(result);
                return time;
            }
            else
            {
                throw new ArgumentException($"Cannot convert string {TimeSpanAs4String} to TimeOnly, it is not in the format HHmm");
            }
        }

        /// <summary>
        /// Takes a string checks for Empty, removes whitespace, clips to the length Len and returns a string
        /// </summary>
        /// <param name="InputString"></param>
        /// <param name="Len"></param>
        /// <returns>string</returns>
        /// /// /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetStringFromString(string InputString, int Len = 0)
        {
            if (string.IsNullOrWhiteSpace(InputString)) return string.Empty;
            string trimmed = InputString.Trim();
            if (Len == 0) return trimmed;
            if (trimmed.Length > Len) return trimmed.Substring(0, Len);
            return trimmed;
        }
    }
}
