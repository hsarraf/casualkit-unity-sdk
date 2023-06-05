using System;
using UnityEngine;


namespace CasualKit.Quick.Ping
{

    public class HostPinger : MonoBehaviour, IPinger
    {
        UnityEngine.Ping Pinger { get; set; }

        public event Action<int> OnPong;

        bool _stopped = false;
        string _host;

        public void StartPing(string host)
        {
            _host = host;
            _stopped = false;
            Ping();
        }

        void Ping()
        {
            Pinger = new UnityEngine.Ping(_host);
            if (Pinger.isDone)
                OnPong?.Invoke(Pinger.time);
            if (!_stopped)
                Invoke(nameof(Ping), 1f);
        }

        public void StopPing()
        {
            _stopped = true;
        }
    }

}