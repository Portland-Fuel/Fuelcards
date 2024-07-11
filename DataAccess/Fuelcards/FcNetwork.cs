using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

/// <summary>
/// A list of networks the fuelcards operate on
/// </summary>
public partial class FcNetwork
{
    public int Id { get; set; }

    public string NetworkName { get; set; } = null!;

    public virtual ICollection<SiteNumberToBand> SiteNumberToBands { get; set; } = new List<SiteNumberToBand>();
}
