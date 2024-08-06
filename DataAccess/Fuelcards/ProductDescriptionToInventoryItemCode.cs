using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class ProductDescriptionToInventoryItemCode
{
    public int Id { get; set; }

    public string? InventoryItemcode { get; set; }

    public string? Description { get; set; }
}
