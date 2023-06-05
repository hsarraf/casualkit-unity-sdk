using UnityEngine;
using CasualKit.Model.Player;
using System;
using CasualKit.Factory;


namespace CasualKit.Api.Auth
{
    public abstract class ApiBehaviour : MonoBehaviour
    {
        [Inject] public IAuth AuthHandler;

        public static ApiBehaviour Instance { get; private set; }
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

        public void Register(string username, string avatar, Action<PlayerModel> onSuccess = null, Action<WebFailResponse> onFail = null)
            => AuthHandler.Register(username, avatar, onSuccess, onFail);
        public void Login(Action<PlayerModel> onSuccess = null, Action<WebFailResponse> onFail = null) =>
            AuthHandler.Login(onSuccess, onFail);

        protected abstract void OnRegistered(PlayerModel playerModel);
        protected abstract void OnRegisterFailed(WebFailResponse error);
        protected abstract void OnLoggedIn(PlayerModel playerModel);
        protected abstract void OnLoginFailed(WebFailResponse error);


        protected virtual void Start()
        {
            AuthHandler.OnRegistered += OnRegistered;
            AuthHandler.OnRegisterFailed += OnRegisterFailed;
            AuthHandler.OnLoggedIn += OnLoggedIn;
            AuthHandler.OnLoginFailed += OnLoginFailed;
        }
    }

}