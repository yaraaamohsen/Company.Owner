using Company.Owner.PL.Setting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
namespace Company.Owner.PL.Helper.EmailSetting
{
    public class MailSrvice(IOptions<MailSettings> _options) : IMailService
    {
        public void SendEmail(Email email)
        {
            // Build Message
            var mail = new MimeMessage();

            var DisplayName = _options.Value.DisplayName;
            var Email = _options.Value.Email;

            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish Connection
            using var smpt = new SmtpClient();
            smpt.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smpt.Authenticate(_options.Value.Email, _options.Value.Password);

            // Send Message
            smpt.Send(mail);

        }
    }
}
