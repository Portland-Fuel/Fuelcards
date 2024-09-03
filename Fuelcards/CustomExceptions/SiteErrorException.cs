
namespace Fuelcards.CustomExceptions
{
    public class SiteErrorException : Exception
    {
        public SiteErrorException(string message)
           : base($"{message}")
        {
        }

        public SiteErrorException(string message, Exception innerException)
            : base($"{message}", innerException)
        {
        }
    }
}
