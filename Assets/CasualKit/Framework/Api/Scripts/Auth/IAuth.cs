using System;
using CasualKit.Model.Player;


namespace CasualKit.Api.Auth
{

    public interface IAuth
    {
        void Register(string username, string avata, Action<PlayerModel> onSuccess = null, Action<WebFailResponse> onFail = null);
        void Login(Action<PlayerModel> onSuccess = null, Action<WebFailResponse> onFail = null);

        event Action<PlayerModel> OnRegistered;
        event Action<WebFailResponse> OnRegisterFailed;
        event Action<PlayerModel> OnLoggedIn;
        event Action<WebFailResponse> OnLoginFailed;

        bool IsLoggedIn { get; }
    }

}