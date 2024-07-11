using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KeyFuelsCard
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public int? AccountNo { get; set; }

    public string? PanNumber { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}
