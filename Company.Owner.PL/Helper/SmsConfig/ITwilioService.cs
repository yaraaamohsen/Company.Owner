using Twilio.Rest.Api.V2010.Account;

namespace Company.Owner.PL.Helper.SmsConfig
{
    public interface ITwilioService
    {
        public MessageResource SendSms(Sms sms);
    }
}
