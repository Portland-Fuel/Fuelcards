using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TexacoCard
{
    public int Id { get; set; }

    public string? Company { get; set; }

    public int? Division { get; set; }

    public string? DivisionName { get; set; }

    public int? ClientNo { get; set; }

    public int? InvoiceCentreNumber { get; set; }

    public int? CustomerNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerCountry { get; set; }

    public decimal? Pan { get; set; }

    public string? Embossed3 { get; set; }

    public int? Embossed4 { get; set; }

    public DateOnly? IssueDate { get; set; }

    public int? IssueNumber { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public char? StopFlag { get; set; }

    public DateOnly? StopDate { get; set; }

    public char? Diesel { get; set; }

    public char? Unleaded { get; set; }

    public char? SuperUnleaded { get; set; }

    public char? Lpg { get; set; }

    public char? CarWash { get; set; }

    public char? Lrp { get; set; }

    public char? Goods { get; set; }

    public char? GasOil { get; set; }

    public char? LubeOil { get; set; }

    public int? PinNo { get; set; }

    public int? CostCenter { get; set; }

    public char? CardLinked { get; set; }

    public string? Uid { get; set; }
}
