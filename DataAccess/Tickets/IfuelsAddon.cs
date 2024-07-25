using System;
using System.Collections.Generic;

namespace DataAccess.Tickets;

public partial class IfuelsAddon
{
    public int Id { get; set; }

    public int? CustomerNumber { get; set; }

    public DateOnly? EffectiveFrom { get; set; }

    public double? Addon { get; set; }

    public int? PaymentTerms { get; set; }

    public int? ModifiedBy { get; set; }

    public bool? Active { get; set; }

    public string? IpAddress { get; set; }

    public DateTime? DateModified { get; set; }
}
