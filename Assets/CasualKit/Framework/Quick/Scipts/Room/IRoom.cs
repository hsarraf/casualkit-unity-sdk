using System;


namespace CasualKit.Quick.Room
{

    public interface IRoom
    {
        void CreateRoom(string roomName, int capacity);
        void JoinRoomByName(string roomName);
        void CreateOrJoinRoom(int capacity);
        void LeaveRoom(string roomName, int[] vidList);

        event Action<QuickStatusObject> OnCheckedIn;
        event Action<CreateRoomObject> OnRoomCreated;
        event Action<string> OnCreateRoomFailed;
        event Action<RoomObject> OnJoinedRoom;
        event Action<string> OnJoinRoomFailed;
        event Action<RoomObject> OnLeftRoom;
        event Action<OppJoinedRoomObject> OnOppJoinedRoom;
        event Action<OppLeftRoomObject> OnOppLeftRoom;
        event Action<OppDisconnectedObject> OnOppDisconnected;
        event Action OnRoomFull;
    }

}