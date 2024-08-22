using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelcardModels.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICreationDate
    {
        /// <summary>
        /// 
        /// </summary>
        public DateOnly? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICreationTime
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IKfDouble
    {
        /// <summary>
        /// 
        /// </summary>
        public double? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IKfInt
    {

        /// <summary>
        /// 
        /// </summary>
        public int? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IFcLong
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICardNumber
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; }

    }
}
