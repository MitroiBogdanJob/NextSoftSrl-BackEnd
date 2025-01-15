using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NextSoftSrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] ContactFormData formData)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mitroibogdandeveloper@gmail.com"), // Schimbă cu adresa ta de email
                    Subject = "New Contact Form Submission",
                    Body = $"Name: {formData.FullName}\nEmail: {"mitroibogdandeveloper@gmail.com"}\nDate: {formData.Date}\nHour: {formData.Hour}",
                    IsBodyHtml = false
                };
                mailMessage.To.Add("mitroibogdandeveloper@gmail.com");

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // Port pentru TLS
                    smtpClient.Credentials = new NetworkCredential("mitroibogdandeveloper@gmail.com", "pgob fiqg bpml pkts"); // Folosește parola de aplicație
                    smtpClient.EnableSsl = true; // Activează SSL

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return Ok(new { message = "Email sent successfully" });
            }
            catch (SmtpException smtpEx)
            {
                return StatusCode(500, new { message = "SMTP Error", error = smtpEx.Message, details = smtpEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending email", error = ex.Message });
            }

        }
    }

    public class ContactFormData
    {
        public string FullName { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
    }
}
