using System;
using UnityEngine;
using CasualKit.Model.Player;
using CasualKit.Factory;
using CasualKit.Model;


namespace CasualKit.Api.Auth
{

    public class ApiAuth : IAuth
    {
        [Inject] IWebRequest<PlayerModel> _WebRequest;
        [Inject] IDataModel _DataModel;
        public ApiAuth() => CKFactory.Inject(this);

        public event Action<PlayerModel> OnRegistered;
        public event Action<WebFailResponse> OnRegisterFailed;
        public event Action<PlayerModel> OnLoggedIn;
        public event Action<WebFailResponse> OnLoginFailed;

        public bool IsLoggedIn => !string.IsNullOrEmpty(_DataModel.PersistentData.UserID) && !string.IsNullOrEmpty(_DataModel.PersistentData.Username);

        public void Register(string username, string avatar, Action<PlayerModel> onSuccess, Action<WebFailResponse> onFail) =>
            _WebRequest.POSTJSON(new { username, avatar },
                CKSettings.Auth.RegisterUrl,
                (response) => {
                    Debug.Log(response.payload.Dump());
                    OnRegistered?.Invoke(response.payload);
                    onSuccess?.Invoke(response.payload);
                },
                (error) => {
                    OnRegisterFailed?.Invoke(error);
                    onFail?.Invoke(error);
                });

        public void Login(Action<PlayerModel> onSuccess, Action<WebFailResponse> onFail)
            => _WebRequest.POSTJSON(new {userId = _DataModel.PersistentData.UserID}, 
                CKSettings.Auth.LoginUrl,
                (response) =>
                {
                    OnLoggedIn?.Invoke(response.payload);
                    onSuccess?.Invoke(response.payload);
                },
                (error) =>
                {
                    OnLoginFailed?.Invoke(error);
                    onFail?.Invoke(error);
                });
    }

}