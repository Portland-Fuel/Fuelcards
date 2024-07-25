using System;
using System.Collections.Generic;

namespace DataAccess.Tickets;

public partial class IfuelsCustomer
{
    public int CustomerNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? Type { get; set; }

    public string? Banding { get; set; }

    public string? AddedBy { get; set; }

    public DateOnly? DateAdded { get; set; }
}
