using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class AppTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationTime { get; set; }

    public short State { get; set; }

    public string? UserId { get; set; }
}
