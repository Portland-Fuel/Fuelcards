using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class UkfuelTransaction
{
    public int? Batch { get; set; }

    public int? Division { get; set; }

    public string? ClientType { get; set; }

    public int? Customer { get; set; }

    public int? Site { get; set; }

    public DateOnly? Transactiondate { get; set; }

    public int? Transactiontime { get; set; }

    public int? Cardnumber { get; set; }

    public string? Registration { get; set; }

    public int? Mileage { get; set; }

    public decimal? Quantity { get; set; }

    public int? Productnumber { get; set; }

    public int? Receiptnumber { get; set; }

    public int? Month { get; set; }

    public int? Week { get; set; }

    public int Transactionnumber { get; set; }

    public decimal? Price { get; set; }

    public decimal? Pan { get; set; }

    public int? Invoiced { get; set; }
}
