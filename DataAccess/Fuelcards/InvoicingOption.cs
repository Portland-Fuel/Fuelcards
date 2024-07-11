using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class InvoicingOption
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public List<int>? GroupedNetwork { get; set; }

    public int? Displaygroup { get; set; }

    public virtual InvoiceFormatGroup? DisplaygroupNavigation { get; set; }
}
