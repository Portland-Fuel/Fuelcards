using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class ConsolidatedTexacoCard
{
    public int Id { get; set; }

    public int PortlandId { get; set; }

    public int AccountNumber { get; set; }

    public string CustomerName { get; set; } = null!;
}
