using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE21Account
{
    public int CustomerAccountCode { get; set; }

    public int? PortlandId { get; set; }

    public short? CustomerAccountSuffix { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public string? ActionStatus { get; set; }

    public string? Name { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Town { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }
}
