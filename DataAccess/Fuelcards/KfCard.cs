using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class KfCard
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public int? Recordtype { get; set; }

    public int? Accountcode { get; set; }

    public string? Accountsuffix { get; set; }

    public decimal? Pannumber { get; set; }

    public int? Stopstatus { get; set; }

    public string? Vehicleregistration { get; set; }

    public string? Embossedtext { get; set; }

    public string? Fourthlineembossedtext { get; set; }

    public bool? Pinrequired { get; set; }

    public bool? Mileagerequired { get; set; }

    public int? Fleetnumber { get; set; }

    public DateOnly? Expirydate { get; set; }

    public int? Pin { get; set; }

    public bool? European { get; set; }

    public bool? Smart { get; set; }

    public int? Singletransfuellimit { get; set; }

    public int? Dailytransfuellimit { get; set; }

    public int? Weeklytransfuellimit { get; set; }

    public int? Notransperday { get; set; }

    public int? Notransperweek { get; set; }

    public int? Nofalsepinentries { get; set; }

    public TimeOnly? Pinlockoutminutes { get; set; }

    public bool? Mondayallowed { get; set; }

    public bool? Tuesdayallowed { get; set; }

    public bool? Wednesdayallowed { get; set; }

    public bool? Thursdayallowed { get; set; }

    public bool? Fridayallowed { get; set; }

    public bool? Saturdayallowed { get; set; }

    public bool? Sundayallowed { get; set; }

    public TimeOnly? Validstarttime { get; set; }

    public TimeOnly? Validendtime { get; set; }
}
