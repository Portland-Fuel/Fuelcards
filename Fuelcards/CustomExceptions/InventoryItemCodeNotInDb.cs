namespace Fuelcards.CustomExceptions
{
    public class InventoryItemCodeNotInDb : Exception
    {
        public InventoryItemCodeNotInDb(string message)
           : base($"{message}")
        {
        }

        public InventoryItemCodeNotInDb(string message, Exception innerException)
            : base($"{message}", innerException)
        {
        }
    }
}
