using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcRequiredEdiReport
{
    public int Id { get; set; }

    public int IntroducerId { get; set; }

    public int CustomerId { get; set; }

    public bool? Keyfuels { get; set; }

    public bool? UkFuels { get; set; }

    public bool? Texaco { get; set; }

    public bool? Fuelgenie { get; set; }
}
