using System;
using CasualKit.Factory;
using CasualKit.Quick.Client;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.Scene;
using CasualKit.Quick.View;
using UnityEngine;


namespace CasualKit.Quick.Room
{

    public class QuickRoom : IRoom
    {

        [Inject] public IClient _client;
        [Inject] public IDispatcher _dispatcher;
        [Inject] public IScene _sceneView;

        public event Action<QuickStatusObject> OnCheckedIn;
        public event Action<CreateRoomObject> OnRoomCreated;
        public event Action<string> OnCreateRoomFailed;
        public event Action<RoomObject> OnJoinedRoom;
        public event Action<string> OnJoinRoomFailed;
        public event Action<RoomObject> OnLeftRoom; // called inside LeaveRoom, not by dispatcher //
        public event Action<OppJoinedRoomObject> OnOppJoinedRoom;
        public event Action<OppLeftRoomObject> OnOppLeftRoom;
        public event Action<OppDisconnectedObject> OnOppDisconnected;
        public event Action OnRoomFull;


        public QuickRoom()
        {
            CKFactory.Inject(this);

            _dispatcher.OnCheckedIn += (stat) =>
            {
                _client.Status.Set((QuickStatus.StateEnum)Enum.Parse(typeof(QuickStatus.StateEnum), stat.status), false);
                OnCheckedIn?.Invoke(stat);
            };
            _dispatcher.OnRoomCreated += (room) =>
            {
                if (room.status == "success")
                {
                    OnRoomCreated?.Invoke(room);
                    _client.Status.Set(QuickStatus.StateEnum.inRoom);
                    _client.Status.Set(room.roomName, room.roomCap);
                }
                else if (room.status == "alreadyExists")
                {
                    OnCreateRoomFailed?.Invoke(room.status);
                }
            };
            _dispatcher.OnJoinedRoom += (room) =>
            {
                if (room.status == "success")
                {
                    OnJoinedRoom?.Invoke(room);
                    _client.Status.Set(QuickStatus.StateEnum.inRoom);
                    _client.Status.Set(room.roomName, room.roomCap);
                }
                else if (room.status == "alreadyExists")
                {
                    OnJoinRoomFailed?.Invoke(room.status);
                }
            };
            _dispatcher.OnOppJoinedRoom += (room) =>
            {
                OnOppJoinedRoom?.Invoke(room);
                _dispatcher.SendToServer(new SyncObject(_sceneView.OppViewsList) { status = "instantiate" });
            };
            _dispatcher.OnOppLeftRoom += (room) =>
            {
                OnOppLeftRoom?.Invoke(room);
                _sceneView.OnDestroyOppObjects(room);
            };
            _dispatcher.OnOppDisconnected += (opp) => OnOppDisconnected(opp);
            _dispatcher.OnRoomFull += () => OnRoomFull();
        }

        public void CreateRoom(string roomName, int capacity)
        {
            if (_client.Status.State == QuickStatus.StateEnum.connected)
                _dispatcher.SendToServer(new CreateRoomObject { roomName = roomName, roomCap = capacity });
            else if (_client.Status.State == QuickStatus.StateEnum.inRoom)
                OnCreateRoomFailed?.Invoke("QUICK, CreateRoom -> player is already present in a room!!");
            else
                OnCreateRoomFailed?.Invoke("QUICK, CreateRoom -> player is not connected yet!!");
        }
        public void JoinRoomByName(string roomName)
        {
            if (_client.Status.State == QuickStatus.StateEnum.connected)
                _dispatcher.SendToServer(new JoinRoomByNameObject { roomName = roomName });
            else if (_client.Status.State == QuickStatus.StateEnum.inRoom)
                OnJoinRoomFailed?.Invoke("NODE, CreateOrJoinRoom -> player is already present in a room!!");
            else
                OnJoinRoomFailed?.Invoke("NODE, CreateOrJoinRoom -> player is not connected yet!!");
        }
        public void CreateOrJoinRoom(int capacity)
        {
            if (_client.Status.State == QuickStatus.StateEnum.connected)
                _dispatcher.SendToServer(new CreateOrJoinRoomObject { roomCap = capacity });
            else if (_client.Status.State == QuickStatus.StateEnum.inRoom)
                OnJoinRoomFailed?.Invoke("NODE, CreateOrJoinRoom -> player is already present in a room!!");
            else
                OnJoinRoomFailed?.Invoke("NODE, CreateOrJoinRoom -> player is not connected yet!!");

        }
        public void LeaveRoom(string roomName, int[] vidList)
        {
            if (_client.Status.State == QuickStatus.StateEnum.inRoom)
            {
                RoomObject roomObject = new LeaveRoomObject { roomName = roomName, vidList = vidList };
                _dispatcher.SendToServer(roomObject);
                OnLeftRoom?.Invoke(roomObject);
                _client.Status.Set(QuickStatus.StateEnum.connected);
            }
            _sceneView.CleanUp();
        }
    }

}