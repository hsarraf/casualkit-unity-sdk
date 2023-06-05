using UnityEngine;
using System;
using System.Collections;
using CasualKit.Model.Player;
using CasualKit.Api;
using CasualKit.Factory;


namespace CasualKit.Quick.Validator
{

    public class DummyValidator : IValidator
    {
        public string UserId { get; set; }
        public string Ticket { get; set; }

        public event Action<string> OnTicketTaken;
        public event Action<string> OnTicketFailed;

        public void IssueTicket(string userId)
        {
            UserId = userId;
            QuickBehaviour.Instance.StartCoroutine(DummyIssue());
        }

        IEnumerator DummyIssue()
        {
            yield return new WaitForSeconds(1f);
            WebResponse<string> ticket = new WebResponse<string>() { status = "success", payload = "tk-dummy" };
            Ticket = ticket.payload;
            OnTicketTaken?.Invoke(ticket.payload);
        }
    }

}
