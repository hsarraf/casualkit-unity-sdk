using System;


namespace CasualKit.Quick.Dispatcher
{

    public interface IDispatcher
    {
        //event Action<QuickStatusObject> _onSocketKilled;

        event Action<BroadcastObject> OnBroadcastReceived;
        event Action<RejoinRoomObject> OnCheckedIn;
        event Action<CreateRoomObject> OnRoomCreated;
        event Action<RoomObject> OnJoinedRoom;
        event Action<OppJoinedRoomObject> OnOppJoinedRoom;
        event Action<OppLeftRoomObject> OnOppLeftRoom;
        event Action<OppDisconnectedObject> OnOppDisconnected;
        event Action<InstantiateObject> OnOppInstantiate;
        event Action<DestroyObjObject> OnOppDestroy;
        event Action<SyncObject> OnSyncOppInstantiations;
        event Action<KillSocketObject> OnSocketKilled;
        event Action OnRoomFull;

        void Broadcast(object data, int vuid);
        void SendToServer(QuickStatusObject data);
    }

}