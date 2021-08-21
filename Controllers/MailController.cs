using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private string MailBody = "<!DOCTYPE html>" +
                                "<html> " +
                                    "<body style=\"background -color:#ff7f26;text-align:center;\"> " +
                                    "<h1 style=\"color:#051a80;\">Welcome to Huy World</h1> " +
                                    "<h2 style=\"color:#fff;\">Please find the attached files.</h2> " +
                                    "<label style=\"color:orange;font-size:100px;border:5px dotted;border-radius:50px\">HUY</label> " +
                                    "</body> " +
                                "</html>";

        private string subject = "Welcome to Huy World.";
        private string mailTitle = "Email from .Net Core App";
        private string fromEmail = "adultsforfuture@gmail.com";
        private string fromEmailPassword = "0399056507ABC";

        [HttpPost]
        public IActionResult Index(string toEmail)
        {
            //Email & Content
            MailMessage message = new MailMessage(new MailAddress(fromEmail, mailTitle), new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = MailBody;

            message.IsBodyHtml = true;

            //Server Details
            SmtpClient smtp = new SmtpClient();
            //Gmail ports - 465 (SSL) or 587 (TLS)
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
            credentials.UserName = fromEmail;
            credentials.Password = fromEmailPassword;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credentials;

            smtp.Send(message);

            return Ok();
        }
    }
}