using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// Logs the requests sent to fuelGenie
/// </summary>
public partial class FgRequestLog
{
    public int Id { get; set; }

    public string Guid { get; set; } = null!;

    public string? Type { get; set; }

    public string? Details { get; set; }

    public string? Result { get; set; }

    public string? Errorinfo { get; set; }

    public DateOnly? RequestDate { get; set; }
}
