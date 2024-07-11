using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class InvoiceReport
{
    public DateOnly InvoiceDate { get; set; }

    public int AccountNo { get; set; }

    public double? DieselVol { get; set; }

    public double? TescoVol { get; set; }

    public double? PetrolVol { get; set; }

    public double? LubesVol { get; set; }

    public double? GasoilVol { get; set; }

    public double? AdblueVol { get; set; }

    public double? PremDieselVol { get; set; }

    public double? SuperUnleadedVol { get; set; }

    public double? SainsburysVol { get; set; }

    public double? OtherVol { get; set; }

    public double? DieselPrice { get; set; }

    public double? TescoPrice { get; set; }

    public double? PetrolPrice { get; set; }

    public double? LubesPrice { get; set; }

    public double? GasoilPrice { get; set; }

    public double? AdbluePrice { get; set; }

    public double? PremDieselPrice { get; set; }

    public double? SuperUnleadedPrice { get; set; }

    public double? SainsburysPrice { get; set; }

    public double? OthersPrice { get; set; }

    public double? Rolled { get; set; }

    public double? Current { get; set; }

    public double? PrevRolled { get; set; }

    public double? DieselLifted { get; set; }

    public double? Fixed { get; set; }

    public double? Floating { get; set; }

    public double? TescoSainsburys { get; set; }

    public double? NetTotal { get; set; }

    public double? Vat { get; set; }

    public double? Total { get; set; }

    public double? Commission { get; set; }

    public int? Network { get; set; }

    public string? InvNo { get; set; }

    public double? BrushTollVol { get; set; }

    public double? BrushTollPrice { get; set; }

    public DateOnly? PayDate { get; set; }

    public string? ComPayable { get; set; }
}
