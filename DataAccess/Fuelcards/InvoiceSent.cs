using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class InvoiceSent
{
    public int Id { get; set; }

    public bool Sent { get; set; }

    public DateOnly Date { get; set; }
}
