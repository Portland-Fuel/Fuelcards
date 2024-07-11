using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FgCardSwipe
{
    public int Id { get; set; }

    public string? Pan { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? RegMileageEntry { get; set; }

    public string? RegCheckInd { get; set; }

    public string? RegCheckNo { get; set; }

    public string? ServiceCode { get; set; }

    public string? PurchaseLimitations { get; set; }

    public string? UserEntry { get; set; }

    public DateOnly? DateSwiped { get; set; }
}
