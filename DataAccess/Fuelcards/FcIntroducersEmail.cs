using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class FcIntroducersEmail
{
    public int Id { get; set; }

    public int IntroducerId { get; set; }

    public int ContactId { get; set; }

    /// <summary>
    /// 0 - Keyfuels, 1 - Uk Fuels, 2 - Texaco, 3 - FuelGenie, 100 - All Networks
    /// </summary>
    public short Network { get; set; }

    /// <summary>
    /// 0 - To, 1 - CC, 2 - BCC
    /// </summary>
    public short EmailField { get; set; }
}
