using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcGrade
{
    public int Id { get; set; }

    public int? GradeId { get; set; }

    public string? Grade { get; set; }
}
