using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcHiddenCard
{
    public int Id { get; set; }

    public string CardNo { get; set; } = null!;

    public int? AccountNo { get; set; }

    public int? PortlandId { get; set; }

    public string? CostCentre { get; set; }

    public int? Network { get; set; }
}
