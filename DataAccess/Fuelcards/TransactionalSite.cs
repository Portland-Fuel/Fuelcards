using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class TransactionalSite
{
    public int Id { get; set; }

    public int SiteCode { get; set; }

    public int Network { get; set; }

    public DateOnly EffectiveDate { get; set; }
}
