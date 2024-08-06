namespace Fuelcards.Models
{
    public class FixedPriceContractVM
    {
        public int Id { get; set; }

        public int? PortlandId { get; set; }

        public DateOnly? EffectiveFrom { get; set; }

        public DateOnly? EndDate { get; set; }

        public double? FixedPrice { get; set; }

        public int? FixedVolume { get; set; }

        public int? Period { get; set; }

        public decimal? TradeReference { get; set; }

        public DateOnly? TerminationDate { get; set; }

        public List<int>? Network { get; set; }

        public double? FixedPriceIncDuty { get; set; }

        public int? Grade { get; set; }

        public int? FrequencyId { get; set; }

        public int? FcAccount { get; set; }
    }
}
