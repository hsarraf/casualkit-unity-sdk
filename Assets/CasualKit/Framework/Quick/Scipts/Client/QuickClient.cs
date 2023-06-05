using System;
using System.Collections;
using UnityEngine;
using CasualKit.Quick.Connection;
using CasualKit.Quick.Connection.WS;
using CasualKit.Quick.Connection.TCP;
using CasualKit.Quick.Settings;
using CasualKit.Quick.Validator;
using CasualKit.Factory;


namespace CasualKit.Quick.Client
{

    public class QuickClient : MonoBehaviour, IClient
    {
        IConnection Connection { get; set; }

        [Inject] IValidator _Validator;
        //readonly IValidator _validator = new QuickFactory<DummyValidator>().Inject();

        public QuickStatus _status;
        public QuickStatus Status { get => _status; set => _status = value; }

        public bool IsConnected => Connection != null ? Connection.IsConnected : false;

        public bool _permanentConnection = true;
        public bool PermanentConnection { get => _permanentConnection; set => _permanentConnection = value; }

        public event Action<string> OnDispatch;
        public event Action OnConnecting;
        public event Action OnConnected;
        public event Action<string> OnError;
        public event Action<CloseStatus> OnClosing;
        public event Action<CloseStatus> OnClosed;
        //public event Action<int> OnPong;

        Coroutine _connectLoopCoroutine = null;
        bool _isTakingTicket = false;

        void Awake()
        {
            CKFactory.Inject(this);
        }

        void Start()
        {
            Status = new QuickStatus();
            InitializeValidator();
        }

        void Update()
        {
            if (Connection != null && Connection.MessageQ.Count > 0)
                OnDispatch?.Invoke(Connection.MessageQ.Dequeue());
        }

        //void InitializePinger()
        //{
        //    Pinger.OnPong += OnPong;
        //}

        void InitializeValidator()
        {
            //_validator = new DummyValidator();
            _Validator.OnTicketTaken += (ticket) =>
            {
                Debug.Log("QUICK: OnTicketTaken, " + ticket);
                Status.Ticket = ticket;
                Connect(ticket, Status.Username, Status.RoomName);
            };
            _Validator.OnTicketFailed += (error) =>
            {
                Status.State = QuickStatus.StateEnum.fail;
                Debug.LogError("QUICK: OnTicketFailed, " + error);
            };
        }

        void InitializeClient(string quickAddress)
        {
            if (CKSettings.Quick.Protocol == QuickSettings.Protocol.TCP)
                Connection = new TCPConnection(quickAddress);
            else if (CKSettings.Quick.Protocol == QuickSettings.Protocol.WS)
                Connection = new WSConnection(quickAddress);

            Connection.OnConnecting += () =>
            {
                Status.State = QuickStatus.StateEnum.connecting;
                OnConnecting?.Invoke();
            };
            Connection.OnConnected += () =>
            {
                Status.State = QuickStatus.StateEnum.connected;
                OnConnected?.Invoke();
                
            };
            Connection.OnError += (error) =>
            {
                Status.State = QuickStatus.StateEnum.fail;
                _isTakingTicket = false;
                OnError?.Invoke(error);
            };
            Connection.OnClosing += (state) =>
            {
                Status.State = QuickStatus.StateEnum.disconnecting;
                _isTakingTicket = false;
                OnClosing?.Invoke(state);
            };
            Connection.OnClosed += (state) =>
            {
                Status.State = QuickStatus.StateEnum.disconnected;
                _isTakingTicket = false;
                OnClosed?.Invoke(state);
            };
        }

        void TerminateClient()
        {
            Connection.OnConnecting -= null;
        }

        public void StartConnection(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                if (_connectLoopCoroutine == null)
                {
                    Status.Username = username;
                    _connectLoopCoroutine = StartCoroutine(ConnectionLoop(username));
                }
            }
            else
                OnError?.Invoke("QUICK: You are not loggedIn yet!!");
        }

        IEnumerator ConnectionLoop(string userId)
        {
            yield return new WaitWhile(() => IsConnected || _isTakingTicket);
            _isTakingTicket = true;
            Status.State = QuickStatus.StateEnum.takingTicket;
            _Validator.IssueTicket(userId);
            yield return new WaitForSeconds(CKSettings.Quick.ConnectLoopRepeatDuration);
            if (PermanentConnection)
                _connectLoopCoroutine = StartCoroutine(ConnectionLoop(userId));
            else
                StopConnection();
        }

        void StopConnection()
        {
            if (_connectLoopCoroutine != null)
            {
                StopCoroutine(_connectLoopCoroutine);
                _connectLoopCoroutine = null;
            }
        }

        public void Connect(string ticket, string userId, string roomName)
        {
            InitializeClient(string.Format("{0}?t_={1}&u_={2}&r_={3}", CKSettings.Quick.ServerAddress, ticket, userId, roomName));
            Connection.Connect();
        }

        public void Send(string data)
        {
            Connection.Send(data);
        }

        public void Close(CloseStatus state, Action onCloseCompleted = null)
        {
            StopConnection();
            //if (IsConnected)
            //{
            //    _dispatcher.SendToServer(new KillSocketObject { status = "normal" });
            //    await Task.Delay(1000);
            //}
            if (Connection != null)
            {
                Connection.Disconnect(state, () =>
                {
                    Connection = null;
                    onCloseCompleted?.Invoke();
                });
            }
            Status.Set(QuickStatus.StateEnum.disconnected);
        }

        private void OnApplicationQuit()
        {
            if (Connection != null)
                Connection.Disconnect();
        }

    }

}
