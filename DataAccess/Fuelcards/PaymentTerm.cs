using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class PaymentTerm
{
    public string XeroId { get; set; } = null!;

    public int? PaymentTerms { get; set; }

    public int? Network { get; set; }
}
