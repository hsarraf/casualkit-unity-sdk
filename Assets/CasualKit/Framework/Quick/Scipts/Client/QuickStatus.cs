using System;
using UnityEngine;


namespace CasualKit.Quick.Client
{

    [Serializable]
    public class QuickStatus
    {
        public enum StateEnum
        {
            None,
            connecting, connected, disconnecting, disconnected,
            takingTicket,
            inRoom,
            fail
        }
        /* status : connecting / connected / OnCheckedIn:[inLobby / inRoom / fail] / disconnected */
        [SerializeField]
        private StateEnum _state = StateEnum.None;
        public StateEnum State { get => _state ; set => _state = value; }

        [SerializeField]
        private string _ticket = null;
        public string Ticket { get => _ticket; set => _ticket = value; }

        [SerializeField]
        private string _username = null;
        public string Username { get => _username; set => _username = value; }

        [SerializeField]
        private string _roomName = null;
        public string RoomName { get => _roomName; set => _roomName = value; }

        [SerializeField]
        private int _roomCap = 0;
        public int RoomCap { get => _roomCap; set => _roomCap = value; }

        public void Set(StateEnum status, bool reset = true)
        {
            State = status;
            if (reset)
            {
                RoomName = null;
                RoomCap = 0;
            }
        }

        public void Set(string roomName, int roomCap)
        {
            RoomName = roomName;
            RoomCap = roomCap;
        }

    }

}