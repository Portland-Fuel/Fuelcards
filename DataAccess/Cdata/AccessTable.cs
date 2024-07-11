using System;
using System.Collections.Generic;

namespace DataAccess.Cdata;

/// <summary>
/// This table shows the user type that require access to the customer database
/// </summary>
public partial class AccessTable
{
    public string UserType { get; set; } = null!;
}
