using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcEmail
{
    public int Account { get; set; }

    public string? To { get; set; }

    public string? Cc { get; set; }

    public string? Bcc { get; set; }
}
