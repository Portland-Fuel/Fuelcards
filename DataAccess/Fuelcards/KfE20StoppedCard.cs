using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE20StoppedCard
{
    public int Id { get; set; }

    public int? CardId { get; set; }

    public int? PortlandId { get; set; }

    public int? CustomerAccountCode { get; set; }

    public short? CustomerAccountSuffix { get; set; }

    public decimal? PanNumber { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public string? StopCode { get; set; }
}
