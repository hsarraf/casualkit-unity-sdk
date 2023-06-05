using UnityEngine;


namespace CasualKit.Quick.Settings
{ 

    [CreateAssetMenu(fileName = "QuickSettings", menuName = "CasualKit/QuickSettings")]
    public class QuickSettings : ScriptableObject
    {
        public enum Protocol
        {
            WS, TCP, UDP, WebRTC
        }
        public Protocol _Protocol = Protocol.TCP;
        // Quick SERVER
        //
        // WS
        //
        [Header("[SERVER ADDRESS]")]
        public string _ServerAddress = "ws://api.segal.games/quick/"; // local develop
        public string _LocalAddress = "ws://127.0.0.1:9000/quick/"; // web develop
        public bool _UseLocal = false;

        // TICKET
        //
        [Header("[VALIDATION]")]
        public string _IssueTicketUrl = "ticket/issue_ticket/";

        // PARAMS
        //
        [Header("[CONNECTION PARAMS]")]
        public int _RecieveBufferLength = 1024;

        public float _ConnectLoopRepeatDuration = 1f;
    }

}