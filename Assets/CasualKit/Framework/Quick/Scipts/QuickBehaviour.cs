using UnityEngine;
using CasualKit.Model;
using System;
using CasualKit.Quick.Connection;
using CasualKit.Quick.Client;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.View;
using CasualKit.Quick.Room;
using CasualKit.Quick.Scene;
using CasualKit.Factory;


namespace CasualKit.Quick
{

    [RequireComponent(typeof(QuickClient))]
    public abstract class QuickBehaviour : MonoBehaviour
    {
        [Inject] public IClient _Client;
        [Inject] public IRoom _RoomView;
        [Inject] public IScene _SceneView;
        [Inject] IDispatcher _Dispatcher;
        [Inject] public IDataModel _Model;

        public static QuickBehaviour Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                CKFactory.Inject(this);
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            RegisterCallbacks();
        }

        void RegisterCallbacks()
        {
            // Client callbacks
            //
            _Client.OnConnected += OnClientConnected;
            _Client.OnError += OnClientError;
            _Client.OnClosed += OnClientClosed;

            // Scene callbacks
            //
            _SceneView.OnOppInstantiated += OnOppInstantiated;
            _SceneView.OnOppDestroyed += OnOppDestroyed;

            // Room callbacks
            //
            _RoomView.OnCheckedIn += OnCheckedIn;
            _RoomView.OnRoomCreated += OnRoomCreated;
            _RoomView.OnCreateRoomFailed += OnCreateRoomFailed;
            _RoomView.OnJoinedRoom += OnJoinedRoom;
            _RoomView.OnJoinRoomFailed += OnJoinRoomFailed;
            _RoomView.OnLeftRoom += OnLeftRoom;
            _RoomView.OnOppJoinedRoom += OnOppJoinedRoom;
            _RoomView.OnOppLeftRoom += OnOppLeftRoom;
            _RoomView.OnOppDisconnected += OnOppDisconnected;
            _RoomView.OnRoomFull += OnRoomIsFull;
        }

        // METHODS //
        /////////////
        public void QuickConnect() => _Client.StartConnection(_Model.PlayerData.username);

        public void QuickDisonnect() => _Client.Close(CloseStatus.NormalClosure);

        public void QuickCreateRoom(string roomName, int roomCapacity) => _RoomView.CreateRoom(roomName, roomCapacity);

        public void QuickJoinRoom(string roomName) => _RoomView.JoinRoomByName(roomName);

        public void CreateOrJoinRoom(int roomCap) => _RoomView.CreateOrJoinRoom(roomCap);

        public void QuickLeaveRoom() => _RoomView.LeaveRoom(_Client.Status.RoomName, _SceneView.OwnerVidsList);

        public QuickView QuickInstantiate(string resPath, Vector3 position, Quaternion rotation) => _SceneView.Instantiate(resPath, position, rotation);

        public void QuickDestroy(QuickView nodeView) => _SceneView.Destroy(nodeView);


        // CALLBAKCS //
        ///////////////
        // CLIENT CALLBAKCS
        ///////////////////
        protected abstract void OnClientConnected();

        protected abstract void OnClientError(string error);

        protected abstract void OnClientClosed(CloseStatus status);

        // SCENE CALLBACKS
        //////////////////
        public abstract void OnOppInstantiated(InstantiateObject instantiateObj);

        public abstract void OnOppDestroyed(SceneObject destroyObj);

        // ROOM CALLBAKCS
        //////////////////
        public abstract void OnCheckedIn(QuickStatusObject status);

        public abstract void OnRoomCreated(RoomObject room);

        public abstract void OnCreateRoomFailed(string err);

        public abstract void OnJoinRoomFailed(string err);

        public abstract void OnJoinedRoom(RoomObject room);

        public abstract void OnLeftRoom(RoomObject room);

        public abstract void OnOppJoinedRoom(OppRoomObject room);

        public abstract void OnOppLeftRoom(OppRoomObject room);

        public abstract void OnOppDisconnected(OppDisconnectedObject opp);

        public abstract void OnRoomIsFull();

    }

}