using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class PortlandFuelcard
{
    public int Id { get; set; }

    public int? Portlandid { get; set; }

    public int? FuelcardAccountNumber { get; set; }

    public string? PanNumber { get; set; }

    public int? Network { get; set; }

    public string? XeroId { get; set; }

    public DateOnly? ExpiryDate { get; set; }
}
