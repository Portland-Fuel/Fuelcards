using System;
using System.Collections.Generic;

namespace DataAccess.Cdata;

public partial class Useraccess
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Company { get; set; }

    public DateOnly? SignupDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? OnTrial { get; set; }

    public DateOnly? TrialendDate { get; set; }

    public string? Telephone { get; set; }

    public string? Password { get; set; }

    public string Username { get; set; } = null!;

    public string? SessionId { get; set; }

    public double? DefaultAddOn { get; set; }

    public string? FuelcardsSession { get; set; }

    public string? TradingSession { get; set; }

    public string? PricesSession { get; set; }

    public string? MicrosoftId { get; set; }
}
