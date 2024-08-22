using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositorys.IRepositorys;
using DataAccess.Fuelcards;
namespace FuelcardModels.Operations
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConvertToDbControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Control"></param>
        /// <returns></returns>
        public static FcControl FileToDb(Control Control, int network)
        {
            FcControl c = new();


            if (Control.CustomerCode is not null && Control.CustomerCode.Value.HasValue) c.CustomerCode = Control.CustomerCode.Value.Value;
            if (Control.CustomerAC is not null && Control.CustomerAC.Value.HasValue) c.CustomerAc = Control.CustomerAC.Value.Value;
            if (Control.CreationDate != null) c.CreationDate = Control.CreationDate.Value;
            if (Control.CreationTime != null) c.CreationTime = Control.CreationTime.Value;
            if (Control.RecordCount != null) c.RecordCount = Control.RecordCount.Value;
            if (Control.BatchNumber != null) c.BatchNumber = Control.BatchNumber.Value;
            if (Control.QuantitySign != null) c.QuantitySign = Control.QuantitySign.Value.ToString();
            if (Control.TotalQuantity != null) c.TotalQuantity = Control.TotalQuantity.Value;
            if (Control.TotalCost != null) c.TotalCost = Control.TotalCost.Value;
            if (Control.TotalCostSign != null) c.CostSign = Control.TotalCostSign.Value.ToString();
            c.Network = network;

            return c;

        }

        
    }
}
