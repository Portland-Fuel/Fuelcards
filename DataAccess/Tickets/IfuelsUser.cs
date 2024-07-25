using System;
using System.Collections.Generic;

namespace DataAccess.Tickets;

public partial class IfuelsUser
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public DateOnly? DateCreated { get; set; }

    public int? ForgotPasswordCode { get; set; }

    public DateOnly? TempPasswordDate { get; set; }

    public bool? Admin { get; set; }

    public bool? EmailConfirmed { get; set; }
}
