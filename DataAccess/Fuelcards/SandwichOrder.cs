using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class SandwichOrder
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public string? User { get; set; }

    public int Size { get; set; }

    public string? Filling { get; set; }

    public int Sauce { get; set; }

    public int BreadType { get; set; }
}
