namespace Fuelcards.Models
{
    public class GenericTransactionFile
    {
        public int TransactionId { get; set; }

        public int ControlId { get; set; }

        public int? PortlandId { get; set; }

        public int? TransactionNumber { get; set; }

        public DateOnly? TransactionDate { get; set; }

        public TimeOnly? TransactionTime { get; set; }

        public int? SiteCode { get; set; }
        public string? CardNumber { get; set; }

        public int? CustomerCode { get; set; }

        public short? CustomerAc { get; set; }

        public string PrimaryRegistration { get; set; }

        public int? Mileage { get; set; }

        public int? FleetNumber { get; set; }

        public short? ProductCode { get; set; }

        public double? Quantity { get; set; }

        public string Sign { get; set; }

        public double? Cost { get; set; }

        public string CostSign { get; set; }

        public string AccurateMileage { get; set; }

        public string CardRegistration { get; set; }

        public string TransactonRegistration { get; set; }

        public bool? Invoiced { get; set; }
        public int? network { get; set; }
    }
}
