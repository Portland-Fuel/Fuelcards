using System;
using System.Collections.Generic;

namespace DataAccess.Tickets;

public partial class PaymentTerm
{
    public int Id { get; set; }

    public int? PaymentTerms { get; set; }
}
