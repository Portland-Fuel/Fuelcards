using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcProspectsDetail
{
    public int Id { get; set; }

    public bool? TpChecled { get; set; }

    public string? Notes { get; set; }

    public string? PrimaryContactName { get; set; }

    public decimal? PrimaryContactNumber { get; set; }

    public virtual FcProspect IdNavigation { get; set; } = null!;
}
