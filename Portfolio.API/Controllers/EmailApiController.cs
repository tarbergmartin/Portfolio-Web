using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Classes;
using Portfolio.Shared.DataModels;

namespace Portfolio.API.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailApiController : ControllerBase
    {
        private readonly SmtpClient _smtpClient;

        public EmailApiController(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        /// <summary>
        /// Endpoint used to send emails from contact form using SendGrid
        /// </summary>
        [HttpPost]
        public IActionResult SendEmail(SubmissionModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Recipient))
            {
                return BadRequest();
            }

            var mailHelper = new MailHelper(_smtpClient);

            var subject = "Digital Portfolio - Contact form";
            var body = new StringBuilder();

            body.Append(string.Format($"<p><strong>Name: </strong>{model.Name}</p>"));
            body.Append(string.Format($"<p><strong>Email: </strong>{model.Email}</p>"));
            body.Append(string.Format("</hr>"));
            body.Append(string.Format("<p><strong>Meddelande:</strong></p>"));
            body.Append(string.Format($"<p>{model.Message}</p>"));

            var success = mailHelper.SendMail(subject, body.ToString(), "noreply@digitalportfolio.com", model.Recipient);

            if (success)
                return Ok();
            else
                return StatusCode(500);
        }
    }
}