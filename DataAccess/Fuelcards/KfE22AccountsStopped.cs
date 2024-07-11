using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE22AccountsStopped
{
    public int Id { get; set; }

    public int CustomerAccountCode { get; set; }

    public int? PortlandId { get; set; }

    public short? CustomerAccountSuffix { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public string? StopStatusCode { get; set; }

    public string? PersonRequestingStop { get; set; }

    public int? StopReferenceNumber { get; set; }
}
