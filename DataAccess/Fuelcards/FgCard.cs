using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// Fuelgenie Portland Cards Details
/// </summary>
public partial class FgCard
{
    public int CardId { get; set; }

    public string PanNumber { get; set; } = null!;

    public int? AccountId { get; set; }

    public string? Status { get; set; }

    public bool? IsActivated { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public int? CardLimit { get; set; }

    public string? DisplayName { get; set; }

    public string? ThirdPartyName { get; set; }

    public bool? CaptureMileagePos { get; set; }

    public bool? IsPoolCard { get; set; }

    public string? EmployeeNumber { get; set; }

    public string? RegionalLocation { get; set; }

    public string? CostCentre { get; set; }

    public string? VehicleType { get; set; }

    public string? RegNo { get; set; }

    public bool? Diesel { get; set; }

    public bool? Unleaded { get; set; }

    public bool? Oil { get; set; }

    public bool? CarWash { get; set; }

    public bool? IsTest { get; set; }

    public int? PortlandId { get; set; }
}
