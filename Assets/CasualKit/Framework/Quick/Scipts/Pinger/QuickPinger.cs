using System;
using System.Collections;
using UnityEngine;


namespace CasualKit.Quick.Ping
{

    public class QuickPinger : MonoBehaviour, IPinger
    {

        public UnityEngine.Ping Pinger { get; set; }

        public event Action<int> OnPong;

        [SerializeField]
        private bool _permanent;
        public bool Permanent { get => _permanent; set => _permanent = value; }

        Coroutine _pingCoroutine = null;

        public void StartPing(string host)
        {
            _pingCoroutine = StartCoroutine(PingCo(host));
        }

        public IEnumerator PingCo(string host)
        {
            Pinger = new UnityEngine.Ping(host);
            yield return new WaitForSeconds(1f);
            if (Pinger.isDone)
                OnPong?.Invoke(Pinger.time);
            if (Permanent)
                _pingCoroutine = StartCoroutine(PingCo(host));
        }

        public void StopPing()
        {
            if (_pingCoroutine != null)
            {
                StopCoroutine(_pingCoroutine);
                _pingCoroutine = null;
            }
        }

    }

}