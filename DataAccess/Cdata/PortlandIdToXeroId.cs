using System;
using System.Collections.Generic;

namespace DataAccess.Cdata;

public partial class PortlandIdToXeroId
{
    public int Id { get; set; }

    public int? PortlandId { get; set; }

    public string XeroId { get; set; } = null!;

    public int? XeroTennant { get; set; }
}
