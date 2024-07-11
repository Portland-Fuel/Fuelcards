using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// Network:
/// 1) Texaco
/// 2) Fuel Genie
/// 3) Key Fuels
/// 4) Uk Fuels
/// </summary>
public partial class Fuelcard
{
    public int PortlandId { get; set; }

    public int? AccountNumber { get; set; }

    public int? Network { get; set; }

    public string? Pan { get; set; }

    public DateOnly? Expiry { get; set; }
}
