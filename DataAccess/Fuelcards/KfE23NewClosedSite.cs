using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE23NewClosedSite
{
    public int SiteAccountCode { get; set; }

    public short? SiteAccountSuffix { get; set; }

    public string? SiteStatus { get; set; }

    public string? Name { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Town { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public string? TelephoneNumber { get; set; }

    public string? ContactName { get; set; }

    public bool? RetailSite { get; set; }

    public bool? Canopy { get; set; }

    public string? MachineType { get; set; }

    public string? OpeningHours1 { get; set; }

    public string? OpeningHours2 { get; set; }

    public string? OpeningHours3 { get; set; }

    public string? Directions { get; set; }

    public int? PoleSignSupplier { get; set; }

    public bool? Parking { get; set; }

    public bool? Payphone { get; set; }

    public bool? Gasoil { get; set; }

    public bool? Showers { get; set; }

    public bool? OvernightAccomodation { get; set; }

    public bool? CafeRestaurant { get; set; }

    public bool? Toilets { get; set; }

    public bool? Shop { get; set; }

    public bool? Lubricants { get; set; }

    public bool? SleeperCabsWelcome { get; set; }

    public bool? TankCleaning { get; set; }

    public bool? Repairs { get; set; }

    public bool? WindscreenReplacement { get; set; }

    public bool? Bar { get; set; }

    public bool? CashpointMachines { get; set; }

    public bool? VehicleClearanceAccepted { get; set; }

    public bool? MotorwayJunction { get; set; }

    public short? MotorwayNumber { get; set; }

    public short? JunctionNumber { get; set; }
}
