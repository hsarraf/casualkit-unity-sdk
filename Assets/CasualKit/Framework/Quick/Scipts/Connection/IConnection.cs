using System;
using System.Collections.Generic;


namespace CasualKit.Quick.Connection
{
    public enum CloseStatus
    {
        Empty = 1005,
        EndpointUnavailable = 1001,
        InternalServerError = 1011,
        InvalidMessageType = 1003,
        InvalidPayloadData = 1007,
        MandatoryExtension = 1010,
        MessageTooBig = 1009,
        NormalClosure = 1000,
        PolicyViolation = 1008,
        ProtocolError = 1002
    }

    public interface IConnection
    {
        void Connect();
        void StartReceiveLoop();
        void Send(string data);
        void Disconnect(CloseStatus status = CloseStatus.NormalClosure, Action onDone = null);

        Uri Address { get; set; }
        bool IsConnected { get; }

        void Dispose();

        Queue<string> MessageQ { get; set; }

        event Action OnConnecting;
        event Action OnConnected;
        //event Action<string> OnReceieved;
        event Action<string> OnError;
        event Action<CloseStatus> OnClosing;
        event Action<CloseStatus> OnClosed;
    }

}