using System.Net;
using System.Net.Mail;

namespace Company.Owner.PL.Helper
{
    public static class EmailSetting
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // Ptotocol : SMTP -> Simple Mail Transfer Protocol
            // Port : 587

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("0yaramohsen00@gmail.com", "roiegmoclicxstkm");
                client.Send("0yaramohsen00@gmail.com", email.To, email.Subject, email.Body);
                
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
