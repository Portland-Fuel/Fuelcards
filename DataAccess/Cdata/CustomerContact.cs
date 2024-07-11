using System;
using System.Collections.Generic;

namespace DataAccess.Cdata;

public partial class CustomerContact
{
    public int? PortlandId { get; set; }

    public int? ContactId { get; set; }

    public DateOnly? DateAdded { get; set; }

    public int? StaffId { get; set; }

    public bool? Addtoemailsout { get; set; }

    public bool? Addtosettlementsemails { get; set; }

    public bool Optinformail { get; set; }

    public DateOnly? Dateoptinformail { get; set; }

    public int? Optinformailsetid { get; set; }

    public DateOnly? Dateoptoutformail { get; set; }

    public int Id { get; set; }
}
