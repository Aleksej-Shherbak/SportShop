using System.Net;
using System.Net.Mail;
using System.Text;
using Domains.Abstract;
using Domains.Entities;

namespace Domains.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "";
        public string MailFromAddress = "";
        public bool UseSsl = true;
        public string Username = "";
        public string Password = "";
        public string ServerName = "";
        public int ServerPort = 578;
        public bool WriteAsFile = false;
        public string FileLocation = "";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly EmailSettings _emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username,
                    _emailSettings.Password);

                if (_emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder();

                body.AppendLine("A new order has been submitted");
                body.AppendLine("---");
                body.AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Quantity * line.Product.Price;

                    body.AppendFormat("{0} x {1} subtotal: {2:c})", line.Quantity, line.Product.Name, subtotal);
                }

                body.AppendLine("---");
                body.AppendLine("Ship to:");
                body.AppendLine(shippingDetails.Name);
                body.AppendLine(shippingDetails.Line1);
                body.AppendLine(shippingDetails.Line2 ?? "");
                body.AppendLine(shippingDetails.Line3 ?? "");
                body.AppendLine(shippingDetails.City);
                body.AppendLine(shippingDetails.Country);
                body.AppendLine(shippingDetails.Zip);

                body.AppendLine("---");
                body.AppendFormat("Gift wrap: {0}", shippingDetails.GiftWrap ? "Yes" : "No");
                
                MailMessage mailMessage = new MailMessage(
                    _emailSettings.MailFromAddress, _emailSettings.MailToAddress,
                    "New order submitted", body.ToString());

                if (_emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }
                
                smtpClient.Send(mailMessage);
            }
        }
    }
}