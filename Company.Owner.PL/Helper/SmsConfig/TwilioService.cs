using Company.Owner.PL.Setting;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Owner.PL.Helper.SmsConfig
{
    public class TwilioService(IOptions<TwilioSetting> _options) : ITwilioService
    {

        public MessageResource SendSms(Sms sms)
        {
            var accountSID = _options.Value.AccountSID;
            var authToken = _options.Value.AuthToken;
             // Intialize Connection
             TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

            // Build Message
            var message = MessageResource.Create(
                body: sms.Body,
                to: sms.To,
                from: _options.Value.PhoneNumber
                );

            // return message
            return message; 
        }
    }
}
