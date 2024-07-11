using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class Product
{
    public int Code { get; set; }

    public string Product1 { get; set; } = null!;

    public int Network { get; set; }
}
