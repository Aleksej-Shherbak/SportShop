using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Domains.Abstract;
using Domains.Entities;
using Microsoft.Extensions.Configuration;

namespace Domains.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly IConfiguration _configuration;

        public EmailOrderProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _configuration.GetValue<bool>("SmtpSettings:UseSsl");
                smtpClient.Host = _configuration["SmtpSettings:Server"];
                smtpClient.Port = _configuration.GetValue<int>("SmtpSettings:Port");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_configuration["SmtpSettings:Username"],
                    _configuration["SmtpSettings:Password"]);

                if (_configuration.GetValue<bool>("SmtpSettings:WriteAsFile"))
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _configuration["SmtpSettings:FileLocation"];
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
                    _configuration["SmtpSettings:MailFromAddress"], _configuration["SmtpSettings:MailToAddress"],
                    "New order submitted", body.ToString());

                if (_configuration.GetValue<bool>("SmtpSettings:WriteAsFile"))
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }
                
                smtpClient.Send(mailMessage);
            }
        }
    }
}