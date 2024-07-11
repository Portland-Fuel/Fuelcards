using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcReseller
{
    public int PortlandId { get; set; }

    public List<int>? Networks { get; set; }
}
