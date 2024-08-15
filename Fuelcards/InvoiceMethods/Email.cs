using PortlandEmail;
namespace Fuelcards.InvoiceMethods
{
    public class Email
    {
        public class SendEmail
        {
            public PortlandEmail.Email message;
            public SendEmail(string userName, string password)
            { 
                message = new PortlandEmail.Email(userName, password);
            }
        }
    }
}
