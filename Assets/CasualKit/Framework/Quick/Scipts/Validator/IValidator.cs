using System;


namespace CasualKit.Quick.Validator
{

    public interface IValidator
    {
        string Ticket { get; set; }

        event Action<string> OnTicketTaken;
        event Action<string> OnTicketFailed;

        public void IssueTicket(string userId);
    }

}