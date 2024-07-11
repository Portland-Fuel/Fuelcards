using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class UkfuelCard
{
    public int Id { get; set; }

    public int? ComapnyNo { get; set; }

    public string? CompanyName { get; set; }

    public int? DivisionNo { get; set; }

    public string? DivisionName { get; set; }

    public int? CustomerNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? CutomerCountry { get; set; }

    public decimal? PanNumber { get; set; }

    public string? Embossed3 { get; set; }

    public string? Embossed4 { get; set; }

    public DateOnly? IssueDate { get; set; }

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

    public string? CardType { get; set; }

    public DateOnly? LastTransaction { get; set; }
}
