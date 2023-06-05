using System;
using Newtonsoft.Json;
using UnityEngine;


namespace CasualKit.Model.Persistent
{

    [System.Serializable]
    public class PersistentModel : PlayerPrefs, IPersistentModel
    {
        public void ExposeOnStart()
        {
            _userId = UserID;
            _username = Username;
        }

        [SerializeField]
        string _userId;
        public string UserID
        {
            get { return GetString("_userid_key_"); }
            set { SetString("_userid_key_", value); _userId = value; }
        }

        [SerializeField]
        string _username;
        public string Username
        {
            get { return GetString("_username_key_"); }
            set { SetString("_username_key_", value); _username = value; }
        }

        public void Update(string userId, string username) => (UserID, Username) = (userId, username);
        public string Dump() => JsonConvert.SerializeObject(this);
        public void Clear() => DeleteAll();
    }

}