using System;
using System.Collections.Generic;

namespace DataAccess.Fuelcards;

public partial class OpsCustomer
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public string? CompanyName { get; set; }

    public string? ContactName { get; set; }

    public string? BuyingFormat { get; set; }

    public string? Address { get; set; }

    public string? BuyingTerms { get; set; }

    public double Premium { get; set; }

    public string? PricingMechanism { get; set; }

    public double CreditLimit { get; set; }

    public virtual ICollection<OpsOrder> OpsOrders { get; set; } = new List<OpsOrder>();
}
