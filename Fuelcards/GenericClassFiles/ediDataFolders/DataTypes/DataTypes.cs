using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuelcardModels.Interfaces;
using FuelcardModels.Utilities;

namespace FuelcardModels.DataTypes
{
    #region Char

    /// <summary>
    /// Action Status
    /// </summary>
    public class ActionStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get;}

        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }

        private char?[] validChars = new char?[] { '4', '5', '9', 'R', 'B', 'C', 'D' };

        /// <summary>
        /// ActionStatus takes one char parameter and assigns it to Value
        /// </summary>
        /// <param name="Value"></param>
        public ActionStatus(char Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// ActionStatus takes one string parameter and assigns the first char to Value if it is valid
        /// <para>String cannot be empty, null, whitespace or have a space as the first character</para>
        /// </summary>
        /// <param name="ActionStatus"></param>
        /// <exception cref="ArgumentException"></exception>
        public ActionStatus(string ActionStatus)
        {
            char? c = Utilities.Utilities.GetCharFromString(ActionStatus);
            if (validChars.Contains(c)) Value = c;
            else throw new ArgumentException($"The Action Status is not one the valid characters {validChars}");
        }
    }

    /// <summary>
    /// Record Type
    /// </summary>
    public class RecordType
    {
        /// <summary>
        /// Value holds the data in the required Type
        /// </summary>
        public char? Value { get; }

        /// <summary>
        /// Text returns the Value as a String
        /// </summary>
        public string Text { get { return Value.ToString(); } }

        /// <summary>
        /// RecordType takes one char parameter and assigns it to Value
        /// </summary>
        /// <param name="Value"></param>
        public RecordType(char Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// RecordType takes one string parameter and assigns the first char to Value
        /// <para>String cannot be empty, null, whitespace or have a space as the first character</para>
        /// </summary>
        /// <param name="RecordType"></param>
        public RecordType(string RecordType)
        {
            Value = Utilities.Utilities.GetCharFromString(RecordType);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TransactionType
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <exception cref="ArgumentOutOfRangeException">If the char is not 3 or 8</exception>
        public TransactionType(char Value)
        {
            this.Value = CheckCharIsValid(Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransactionType"></param>
        /// <exception cref="ArgumentOutOfRangeException">If the char is not 3 or 8</exception>
        public TransactionType(string TransactionType)
        {
            char? c = Utilities.Utilities.GetCharFromString(TransactionType);
            Value = CheckCharIsValid(c);
        }

        private char? CheckCharIsValid(char? c)
        {
            int i = (int)c;
            if (i < 49 || i > 57) throw new ArgumentOutOfRangeException($"The Transaction Type should be a number between 1 and 9 and not {c}");
            return c;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Sign
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get; } = '+';
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Sign(char Value)
        {
            if (Value == ' ') return;
            if (Value == '+' || Value =='-') this.Value = Value;
            else throw new ArgumentException($"Sign must be either '+' or '-' but not {Value}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sign"></param>
        public Sign(string Sign)
        {
            Value = Utilities.Utilities.GetCharFromString(Sign);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccurateMileage
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public AccurateMileage(char Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccurateMileage"></param>
        public AccurateMileage(string AccurateMileage)
        {
            Value = Utilities.Utilities.GetCharFromString(AccurateMileage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HandlingCharge
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public HandlingCharge(char Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HandlingCharge"></param>
        public HandlingCharge(string HandlingCharge)
        {
            Value = Utilities.Utilities.GetCharFromString(HandlingCharge);
        }
    }

    /// <summary>
    /// GenericChar can take either a char or string and saves the value as a char
    /// </summary>
    public class GenericChar
    {
        /// <summary>
        /// 
        /// </summary>
        public char? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get { return Value.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public GenericChar(char Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GenericChar"></param>
        public GenericChar(string GenericChar)
        {
            Value = Utilities.Utilities.GetCharFromString(GenericChar);
        }
    }
    #endregion

    #region string

    #region KeyFuels Strings
    /// <summary>
    /// 
    /// </summary>
    public class AddressLine1
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public AddressLine1(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressLine2
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public AddressLine2(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ContactName
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public ContactName(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 29);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CardRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CardRegistration(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 12);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CardNumber : ICardNumber
    {
        /// <summary>
        /// Stores the PAN / Card number as a string
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DecimalAsString"></param>
        public CardNumber(string DecimalAsString)
        {
            this.Value = Utilities.Utilities.GetDecimalFromString(DecimalAsString, 19, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CardNumber(decimal Value)
        {
            this.Value = Utilities.Utilities.CheckDecimalDimensions(Value, 19, 0);
        }

        ///// <summary>
        ///// ToString returns the Value as a string or an empty string
        ///// </summary>
        ///// <returns>string</returns>
        //public override string ToString()
        //{
        //    if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
        //    return Value;
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public class County
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public County(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 20);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustOwnOrderNo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CustOwnOrderNo(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 15);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomerOrderNo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CustomerOrderNo(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 15);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeliveryNoteNo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public DeliveryNoteNo(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 10);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Directions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Directions(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 225);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmbossingDetails
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public EmbossingDetails(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 21);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Name
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Name(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Narrative
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Narrative(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 70);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpeningHours1
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public OpeningHours1(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 21);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpeningHours2
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public OpeningHours2(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 21);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpeningHours3
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public OpeningHours3(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 21);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PAN
    {
        /// <summary>
        /// Stores the PAN number as a string
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public PAN(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 19);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PostCode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public PostCode(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 10);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PrimaryRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public PrimaryRegistration(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 12);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public ProductDescription(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Reference(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 5);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SupplierName
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public SupplierName(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TelephoneNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public TelephoneNumber(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 15);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Town
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Town(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 30);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VehicleRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public VehicleRegistration(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 12);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    #endregion

    #region UK Fuels and Texaco Strings

    

    

    /// <summary>
    /// 
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Registration(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 12);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReceiptNo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public ReceiptNo(string Value)
        {
            this.Value = Utilities.Utilities.GetStringFromString(Value, 4);
        }

        /// <summary>
        /// ToString returns the Value as a string or an empty string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return string.Empty;
            return Value;
        }
    }

    #endregion
    #endregion

    #region int
    /// <summary>
    /// 
    /// </summary>
    public class Int1
    {
        /// <summary>
        /// 
        /// </summary>
        public sbyte? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int1(int Value)
        {
            this.Value = (sbyte)Utilities.Utilities.CheckIntForSize(Value, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int1(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetSbyteFromString(IntAsString, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int2
    {
        /// <summary>
        /// 
        /// </summary>
        public sbyte? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int2(int Value)
        {
            this.Value = (sbyte)Utilities.Utilities.CheckIntForSize(Value, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SbyteAsString"></param>
        public Int2(string SbyteAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetSbyteFromString(SbyteAsString, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int3 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int3(int Value)
        {
            this.Value = (short)Utilities.Utilities.CheckIntForSize(Value, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShortAsString"></param>
        public Int3(string ShortAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetShortFromString(ShortAsString, 3);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int4 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int4(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int4(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 4);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int5 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get 
            {
                if (Value == null) return string.Empty;
                return Value.ToString(); 
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int5(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int5(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 5);
            }catch(ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int6 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int6(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 6);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int6(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 6);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int7 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int7(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 7);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int7(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 7);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int8 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int8(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int8(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 8);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Int9 : IKfInt
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Int9(int Value)
        {
            this.Value = Utilities.Utilities.CheckIntForSize(Value, 9);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntAsString"></param>
        public Int9(string IntAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetIntFromString(IntAsString, 9);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }


    #endregion

    #region long

    /// <summary>
    /// 
    /// </summary>
    public class Long10 : IFcLong
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Long10(long Value)
        {
            this.Value = Utilities.Utilities.CheckLongForSize(Value, 10);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LongAsString"></param>
        public Long10(string LongAsString)
        {
            try
            {
                Value = Utilities.Utilities.GetLongFromString(LongAsString, 10);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }


    #endregion

    #region double
    /// <summary>
    /// 
    /// </summary>
    public class Double11 : IKfDouble
    {
        /// <summary>
        /// 
        /// </summary>
        public double? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get 
            {
                if (Value == null) return string.Empty;
                return Value.Value.ToString("0.00"); 
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Double11(double Value)
        {
            this.Value = Utilities.Utilities.CheckDoubleDimensions(Value, 11, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Double11"></param>
        public Double11(string Double11)
        {
            try
            {
                Value = Utilities.Utilities.GetDoubleFromString(Double11, 11, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Double9 : IKfDouble
    {
        /// <summary>
        /// 
        /// </summary>
        public double? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.Value.ToString("0.00");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Double9(double Value)
        {
            this.Value = Utilities.Utilities.CheckDoubleDimensions(Value, 9, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Double9"></param>
        public Double9(string Double9)
        {
            try
            {
                Value = Utilities.Utilities.GetDoubleFromString(Double9, 9, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Double7 : IKfDouble
    {
        /// <summary>
        /// 
        /// </summary>
        public double? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.Value.ToString("0.00");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Double7(double Value)
        {
            this.Value = Utilities.Utilities.CheckDoubleDimensions(Value, 7, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Double7"></param>
        public Double7(string Double7)
        {
            try
            {
                Value = Utilities.Utilities.GetDoubleFromString(Double7, 7, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Double5 : IKfDouble
    {
        /// <summary>
        /// 
        /// </summary>
        public double? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == null) return string.Empty;
                return Value.Value.ToString("0.00");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Double5(double Value)
        {
            this.Value = Utilities.Utilities.CheckDoubleDimensions(Value, 5, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Double5"></param>
        public Double5(string Double5)
        {
            try
            {
                Value = Utilities.Utilities.GetDoubleFromString(Double5, 5, 2);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    
    #endregion

    #region decimal



    /// <summary>
    /// 
    /// </summary>
    public class Decimal20 : ICardNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { 
            get 
            {
                if (Value == 0) return string.Empty;
                return Value.ToString(); 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public Decimal20(decimal Value)
        {
            this.Value = Utilities.Utilities.CheckDecimalDimensions(Value, 20);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Decimal20"></param>
        public Decimal20(string Decimal20)
        {
            try
            {
                Value = Utilities.Utilities.GetDecimalFromString(Decimal20, 20, 0);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// Keyfuels use a 19 digit Card Number
    /// </summary>
    public class CardNumber19 : ICardNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == 0) return string.Empty;
                return Value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CardNumber19(decimal Value)
        {
            this.Value = Utilities.Utilities.CheckDecimalDimensions(Value, 19, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Decimal19"></param>
        public CardNumber19(string Decimal19)
        {
            try
            {
                Value = Utilities.Utilities.GetDecimalFromString(Decimal19, 19, 0);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// UKfuels use a 20 digit Card Number
    /// </summary>
    public class CardNumber20 : ICardNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == 0) return string.Empty;
                return Value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CardNumber20(decimal Value)
        {
            this.Value = Utilities.Utilities.CheckDecimalDimensions(Value, 20, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Decimal20"></param>
        public CardNumber20(string Decimal20)
        {
            try
            {
                Value = Utilities.Utilities.GetDecimalFromString(Decimal20, 20, 0);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }

    /// <summary>
    /// Texaco use a 7 digit Card Number
    /// </summary>
    public class CardNumber7 : ICardNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value == 0) return string.Empty;
                return Value.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public CardNumber7(decimal Value)
        {
            this.Value = Utilities.Utilities.CheckDecimalDimensions(Value, 7, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Decimal7"></param>
        public CardNumber7(string Decimal7)
        {
            try
            {
                Value = Utilities.Utilities.GetDecimalFromString(Decimal7, 7, 0);
            }
            catch (ArgumentNullException)
            {
                Value = null;
            }
        }
    }


    #endregion

    #region DateTime

    /// <summary>
    /// 
    /// </summary>
    public class DateTime10 : ICreationDate
    {
        /// <summary>
        /// 
        /// </summary>
        public DateOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value.HasValue) return Value.Value.ToString("yyyy-MM-dd");
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public DateTime10(DateOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateTime10"></param>
        public DateTime10(string DateTime10)
        {
            Value = Utilities.Utilities.GetDateOnlyFromString(DateTime10);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateOnly8 : ICreationDate
    {
        /// <summary>
        /// 
        /// </summary>
        public DateOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { 
            get { 
                if(Value.HasValue) return Value.Value.ToString("yyyyMMdd");
                return string.Empty;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public DateOnly8(DateOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateTime8"></param>
        public DateOnly8(string DateTime8)
        {
            Value = Utilities.Utilities.GetDateOnlyFrom8String(DateTime8);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateOnly6 : ICreationDate
    {
        /// <summary>
        /// 
        /// </summary>
        public DateOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (Value.HasValue) return Value.Value.ToString("ddMMyy");
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public DateOnly6(DateOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateTime6"></param>
        public DateOnly6(string DateTime6)
        {
            Value = Utilities.Utilities.GetDateOnlyFrom6String(DateTime6);
        }
    }

    #endregion

    #region TimeSpan
          
    /// <summary>
    /// 
    /// </summary>
    public class TimeOnly8 : ICreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text 
        { 
            get 
            { 
                if(Value.HasValue) return Value.Value.ToString("hh\\:mm\\:ss");
                return string.Empty;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public TimeOnly8(TimeOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeOnly8"></param>
        public TimeOnly8(string TimeOnly8)
        {
            Value = Utilities.Utilities.GetTimeOnlyFromString(TimeOnly8);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeOnly6 : ICreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text 
        { 
            get 
            {
                if (Value.HasValue) return Value.Value.ToString("hhmmss");
                return string.Empty;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public TimeOnly6(TimeOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeOnly6"></param>
        public TimeOnly6(string TimeOnly6)
        {
            Value = Utilities.Utilities.GetTimeOnlyFrom6String(TimeOnly6);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeOnly4 : ICreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly? Value { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Text 
        { 
            get 
            { 
                if (Value.HasValue) return Value.Value.ToString("hhmm");
                return string.Empty;
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public TimeOnly4(TimeOnly Value)
        {
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeOnly4"></param>
        public TimeOnly4(string TimeOnly4)
        {
            Value = Utilities.Utilities.GetTimeOnlyFrom4String(TimeOnly4);
        }
    }

    #endregion

    #region Spare

    /// <summary>
    /// Spare can be called without instantiating it
    /// </summary>
    public static class Spare
    {
        /// <summary>
        /// SpareString returns an empty string
        /// </summary>
        /// <returns>Empty string</returns>
        public static string SpareString { get { return string.Empty; } }
    }
    #endregion
}
