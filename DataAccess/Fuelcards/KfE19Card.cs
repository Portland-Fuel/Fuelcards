using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfE19Card
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public int? CustomerAccountCode { get; set; }

    public short? CustomerAccountSuffix { get; set; }

    public decimal? PanNumber { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public string? ActionStatus { get; set; }

    public string? OdometerUnit { get; set; }

    public string? VehicleReg { get; set; }

    public string? EmbossingDetails { get; set; }

    public short? CardGrade { get; set; }

    public string? MileageEntryFlag { get; set; }

    public bool? PinRequired { get; set; }

    public short? PinNumber { get; set; }

    public bool? TelephoneRequired { get; set; }

    public short? ExpiryDate { get; set; }

    public bool? European { get; set; }

    public bool? Smart { get; set; }

    public int? SingleTransFuelLimit { get; set; }

    public int? DailyTransFuelLimit { get; set; }

    public int? WeeklyTransFuelLimit { get; set; }

    public int? NumberTransPerDay { get; set; }

    public int? NumberTransPerWeek { get; set; }

    public short? NumberFalsePinEntries { get; set; }

    public short? PinLockoutMinutes { get; set; }

    public bool? MondayAllowed { get; set; }

    public bool? TuesdayAllowed { get; set; }

    public bool? WednesdayAllowed { get; set; }

    public bool? ThursdayAllowed { get; set; }

    public bool? FridayAllowed { get; set; }

    public bool? SaturdayAllowed { get; set; }

    public bool? SundayAllowed { get; set; }

    public short? ValidStartTime { get; set; }

    public short? ValidEndTime { get; set; }
}
