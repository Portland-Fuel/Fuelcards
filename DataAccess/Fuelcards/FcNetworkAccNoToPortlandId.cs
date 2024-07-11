using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// the networks are as follows 0 - Keyfuels, 1 - UK Fuels, 2 - Texaco, 3 - FuelGenie
/// </summary>
public partial class FcNetworkAccNoToPortlandId
{
    public int Id { get; set; }

    public int FcAccountNo { get; set; }

    public short Network { get; set; }

    public int PortlandId { get; set; }
}
