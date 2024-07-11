using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TexacoEdi
{
    public int Id { get; set; }

    public int? WeekNo { get; set; }

    public int? DistNo { get; set; }

    public char? CustType { get; set; }

    public int? CustNo { get; set; }

    public int? SiteNo { get; set; }

    public DateOnly? Date { get; set; }

    public DateTimeOffset? Time { get; set; }

    public int? CardNo { get; set; }

    public string? Reg { get; set; }

    public string? BunkerSheet { get; set; }

    public int? Mileage { get; set; }

    public int? Quantity { get; set; }

    public int? ProductNo { get; set; }

    public int? MonthNo { get; set; }

    public int? WeekNoChange { get; set; }

    public int? TransactionNo { get; set; }

    public int? Value { get; set; }

    public int? HostAccount { get; set; }
}
