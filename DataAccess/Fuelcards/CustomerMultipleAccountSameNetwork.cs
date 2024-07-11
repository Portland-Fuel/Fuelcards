using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class CustomerMultipleAccountSameNetwork
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public int? FcAccount { get; set; }

    public int? Network { get; set; }
}
