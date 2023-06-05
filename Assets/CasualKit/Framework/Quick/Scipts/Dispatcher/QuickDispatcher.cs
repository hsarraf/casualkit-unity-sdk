using System;
using CasualKit.Factory;
using CasualKit.Quick.Client;
using UnityEngine;


namespace CasualKit.Quick.Dispatcher
{

    public class QuickDispatcher : IDispatcher
    {
        [Inject] IClient _Client;

        // Operation Flags//
        ////////////////////
        public const char OPT_VALIDATION = 'v'; /* Just for TCP */
        public const char OPT_CHECKED_IN = 'h';
        public const char OPT_BROADCAST = 'b';
        public const char OPT_CREATE_ROOM = 'c';
        public const char OPT_CREATE_OR_JOIN_ROOM = 'r';
        public const char OPT_JOIN_ROOM_BY_NAME = 'j';
        public const char OPT_OPP_JOINED_ROOM = 'o';
        public const char OPT_LEAVE_ROOM = 'e';
        public const char OPT_OPP_LEFT_ROOM = 'm';
        public const char OPT_OPP_DISCONNECTED = 'x';
        public const char OPT_INSTANTIATE = 'i';
        public const char OPT_DESTROY = 'y';
        public const char OPT_SYNC = 's';
        public const char OPT_KILL_SOCKET = 'k';
        public const char OPT_ROOM_FULL = 'l';
        public const char OPT_PING = 'p';

        // callbacks //
        ///////////////
        public event Action<BroadcastObject> OnBroadcastReceived;
        public event Action<RejoinRoomObject> OnCheckedIn;
        public event Action<CreateRoomObject> OnRoomCreated;
        public event Action<RoomObject> OnJoinedRoom;
        public event Action<OppJoinedRoomObject> OnOppJoinedRoom;
        public event Action<OppLeftRoomObject> OnOppLeftRoom;
        public event Action<OppDisconnectedObject> OnOppDisconnected;
        public event Action<InstantiateObject> OnOppInstantiate;
        public event Action<DestroyObjObject> OnOppDestroy;
        public event Action<SyncObject> OnSyncOppInstantiations;
        public event Action<KillSocketObject> OnSocketKilled;
        public event Action OnRoomFull;


        public QuickDispatcher()
        {
            CKFactory.Inject(this);
            _Client.OnDispatch += Dispatch;
        }


        void Dispatch(string data)
        {
            char opt = data[0];
            if (opt == OPT_BROADCAST)
            {
                OnBroadcastReceived?.Invoke(BroadcastObject.Load(data[1..]));
                return;
            }
            if (opt == OPT_CHECKED_IN)
            {
                OnCheckedIn?.Invoke(QuickStatusObject.Load<RejoinRoomObject>(data[1..]));
            }
            else if (opt == OPT_CREATE_ROOM)
            {
                OnRoomCreated?.Invoke(QuickStatusObject.Load<CreateRoomObject>(data[1..]));
            }
            else if (opt == OPT_JOIN_ROOM_BY_NAME)
            {
                OnJoinedRoom?.Invoke(QuickStatusObject.Load<JoinRoomByNameObject>(data[1..]));
            }
            else if (opt == OPT_CREATE_OR_JOIN_ROOM)
            {
                OnJoinedRoom?.Invoke(QuickStatusObject.Load<CreateOrJoinRoomObject>(data[1..]));
            }
            else if (opt == OPT_OPP_JOINED_ROOM)
            {
                OnOppJoinedRoom?.Invoke(QuickStatusObject.Load<OppJoinedRoomObject>(data[1..]));
            }
            else if (opt == OPT_OPP_LEFT_ROOM)
            {
                OnOppLeftRoom?.Invoke(QuickStatusObject.Load<OppLeftRoomObject>(data[1..]));
            }
            else if (opt == OPT_OPP_DISCONNECTED)
            {
                OnOppDisconnected?.Invoke(QuickStatusObject.Load<OppDisconnectedObject>(data[1..]));
            }
            else if (opt == OPT_INSTANTIATE)
            {
                OnOppInstantiate?.Invoke(QuickStatusObject.Load<InstantiateObject>(data[1..]));
            }
            else if (opt == OPT_DESTROY)
            {
                OnOppDestroy?.Invoke(QuickStatusObject.Load<DestroyObjObject>(data[1..]));
            }
            else if (opt == OPT_SYNC)
            {
                OnSyncOppInstantiations?.Invoke(QuickStatusObject.Load<SyncObject>(data[1..]));
            }
            else if (opt == OPT_KILL_SOCKET)
            {
                OnSocketKilled?.Invoke(QuickStatusObject.Load<KillSocketObject>(data[1..]));
            }
            else if (opt == OPT_ROOM_FULL)
            {
                OnRoomFull?.Invoke();
            }
            else if (opt == OPT_VALIDATION)  /* Just for TCP */
            {
                SendToServer(new ValidationObject
                {
                    status = "validation",
                    ticket = _Client.Status.Ticket,
                    username = _Client.Status.Username,
                    roomName = _Client.Status.RoomName
                });
            }
            else if (opt == OPT_PING)
            {
                //Pinger.OnPong?.Invoke(1);
            }
            else
            {
                _Client.Close(Connection.CloseStatus.InvalidMessageType);
            }
        }

        public void Broadcast(object data, int vuid)
        {
            if (_Client != null && _Client.Status.State == QuickStatus.StateEnum.inRoom)
                _Client.Send(new BroadcastObject(data, vuid).Dump());
        }

        public void SendToServer(QuickStatusObject data)
        {
            if (_Client != null)
                _Client.Send(data.Dump());
        }

    }

}