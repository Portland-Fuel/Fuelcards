using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class InvoiceFormatGroup
{
    public int Id { get; set; }

    public string? Group { get; set; }

    public virtual ICollection<InvoicingOption> InvoicingOptions { get; set; } = new List<InvoicingOption>();
}
