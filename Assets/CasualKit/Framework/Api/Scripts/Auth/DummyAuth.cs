using System;
using System.Collections;
using CasualKit.Model;
using CasualKit.Model.Player;
using UnityEngine;


namespace CasualKit.Api.Auth
{

    public class DummyAuth : IAuth
    {
        public event Action<PlayerModel> OnRegistered;
        public event Action<WebFailResponse> OnRegisterFailed;
        public event Action<PlayerModel> OnLoggedIn;
        public event Action<WebFailResponse> OnLoginFailed;

        string _dummyUserId;
        string _dummyUsername;

        public bool IsLoggedIn => false;

        public void Register(string username, string gender, Action<PlayerModel> onSuccess, Action<WebFailResponse> onFail)
        {
            _dummyUsername = username;
            ApiBehaviour.Instance.StartCoroutine(DummyRegister(onSuccess, onFail));
        }
        IEnumerator DummyRegister(Action<PlayerModel> onSuccess, Action<WebFailResponse> onFail)
        {
            yield return new WaitForSeconds(1f);
            OnRegistered?.Invoke(new PlayerModel { userId = _dummyUserId, username = _dummyUsername, profile = null, social = null, payment = null });
            onSuccess?.Invoke(new PlayerModel { userId = _dummyUserId, username = _dummyUsername, profile = null, social = null, payment = null });
        }

        public void Login(Action<PlayerModel> onSuccess, Action<WebFailResponse> onFail)
        {
            _dummyUsername = ModelBehaviour.Instance.GetComponent<DataModel>().PersistentData.Username;
            ApiBehaviour.Instance.StartCoroutine(DummyLogin());
        }
        IEnumerator DummyLogin()
        {
            yield return new WaitForSeconds(1f);
            OnLoggedIn?.Invoke(new PlayerModel { userId = _dummyUserId, username = _dummyUsername, profile = null, social = null, payment = null });
        }
    }

}