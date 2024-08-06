namespace Fuelcards.Models
{
    public class GenericTransactionFile
    {
        public int transactionId { get; set; }

        public int controlId { get; set; }

        public int? portlandId { get; set; }

        public int? transactionNumber { get; set; }

        public DateOnly? transactionDate { get; set; }

        public TimeOnly? transactionTime { get; set; }

        public int? siteCode { get; set; }
        public string? cardNumber { get; set; }

        public int? customerCode { get; set; }

        public short? customerAc { get; set; }

        public string? primaryRegistration { get; set; }

        public int? mileage { get; set; }

        public int? fleetNumber { get; set; }

        public short? productCode { get; set; }

        public double? quantity { get; set; }

        public string? sign { get; set; }

        public double? cost { get; set; }

        public string? costSign { get; set; }

        public string? accurateMileage { get; set; }

        public string? cardRegistration { get; set; }

        public string? transactonRegistration { get; set; }

        public bool? invoiced { get; set; }
        public int? network { get; set; }

        public string? siteName { get; set; }
        public double? invoicePrice { get; set; }
        public double? unitPrice { get; set; }
        public string? product { get; set; }
        public string band { get; set; }

    }
}
