using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FgCardstatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }
}
