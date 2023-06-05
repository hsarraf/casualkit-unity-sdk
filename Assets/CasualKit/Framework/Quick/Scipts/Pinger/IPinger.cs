using System;


namespace CasualKit.Quick.Ping
{

    public interface IPinger
    {
        void StartPing(string host);
        void StopPing();

        event Action<int> OnPong;

    }

}