using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Helper.EmailSetting
{
    public class Email
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
