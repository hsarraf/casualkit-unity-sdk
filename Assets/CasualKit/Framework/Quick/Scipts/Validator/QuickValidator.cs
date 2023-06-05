using System;
using CasualKit.Api;
using CasualKit.Factory;


namespace CasualKit.Quick.Validator
{
    public class QuickValidator : IValidator
    {
        [Inject] IWebRequest<string> _webRequest;
        QuickValidator() => CKFactory.Inject(this);


        public string Ticket { get; set; }

        public event Action<string> OnTicketTaken;
        public event Action<string> OnTicketFailed;

        public void IssueTicket(string userId)
        {
            _webRequest.POSTJSON((appId : CKSettings.Api.ApiKey, userId ),
                CKSettings.Quick.IssueTicketUrl,
                (ticket) => { Ticket = ticket.payload; OnTicketTaken?.Invoke(ticket.payload); },
                (error) => { Ticket = null; OnTicketFailed?.Invoke(error.error); });
        }
    }
}

