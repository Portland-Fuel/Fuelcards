using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class MissingProductValue
{
    public int Id { get; set; }

    public int Network { get; set; }

    public int? Product { get; set; }

    public double? Value { get; set; }
}
