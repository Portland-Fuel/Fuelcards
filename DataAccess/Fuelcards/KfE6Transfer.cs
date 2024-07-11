using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE6Transfer
{
    public int Id { get; set; }

    public int? TransactionNumber { get; set; }

    public short? TransactionSequence { get; set; }

    public string? Narrative { get; set; }

    public int? ControlId { get; set; }
}
