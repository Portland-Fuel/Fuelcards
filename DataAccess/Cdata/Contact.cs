using System;
using System.Collections.Generic;

namespace DataAccess.Cdata;

public partial class Contact
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Mobile { get; set; }

    public string? Fax { get; set; }

    public bool Optinforemail { get; set; }

    public DateOnly? Dateoptinforemail { get; set; }

    public int? Optinemailsetid { get; set; }

    public bool Optinforphone { get; set; }

    public int? Optinphonesetid { get; set; }

    public DateOnly? Dateoptinforphone { get; set; }

    public DateOnly? Dateoptoutforemail { get; set; }

    public DateOnly? Dateoptoutforphone { get; set; }

    public string? Salutation { get; set; }
}
