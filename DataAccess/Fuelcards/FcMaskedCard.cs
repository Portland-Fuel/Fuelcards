using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// A table for masked cards numbers and the portland id they relate to
/// </summary>
public partial class FcMaskedCard
{
    public decimal CardNumber { get; set; }

    public int PortlandId { get; set; }

    public short Network { get; set; }
}
