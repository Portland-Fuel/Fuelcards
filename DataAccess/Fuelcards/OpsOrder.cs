using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class OpsOrder
{
    public int Id { get; set; }

    public short OrderState { get; set; }

    public int CustomerId { get; set; }

    public DateTime DeliveryDate { get; set; }

    public DateTime OrderDate { get; set; }

    public int Volume { get; set; }

    public DateTime CreationTime { get; set; }

    public virtual OpsCustomer Customer { get; set; } = null!;
}
