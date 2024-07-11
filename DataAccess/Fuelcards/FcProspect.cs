using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcProspect
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual FcProspectsDetail? FcProspectsDetail { get; set; }
}
