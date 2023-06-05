using System;
using CasualKit.Quick.Connection;


namespace CasualKit.Quick.Client
{

    public interface IClient
    {
        public QuickStatus Status { get; set; }

        bool IsConnected { get; }
        bool PermanentConnection { get; set; }

        // methods
        //
        void StartConnection(string userId);
        void Connect(string ticket, string userId, string roomName);
        void Send(string data);
        void Close(CloseStatus state, Action onClosed = null);

        // events
        //
        event Action<string> OnDispatch;
        event Action OnConnecting;
        event Action OnConnected;
        event Action<string> OnError;
        event Action<CloseStatus> OnClosing;
        event Action<CloseStatus> OnClosed;

        //event Action<int> OnPong;

    }

}