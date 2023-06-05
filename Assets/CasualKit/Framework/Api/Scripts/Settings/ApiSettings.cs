using UnityEngine;

namespace CasualKit.Api.Settings
{

    [CreateAssetMenu(fileName = "ApiSettings", menuName = "CasualKit/ApiSettings")]
    public class ApiSettings : ScriptableObject
    {
        //App ID
        //
        public string _ApiKey = "Iyt17TbILJ0sX78FbFtzYtWj5pMYpE2L8OTZRc5WQs61RjCkKvXVfn5lxkvE1PRx";

        // API SERVER
        //
        [Header("[REQUEST URLS]")]
        public string _ServerAddress = "http://api.segal.games/dev/"; // web develop
        public string _LocalAddress = "http://127.0.0.1:8000/dev/"; // web develop
        public bool _Local = false;

        // LOGS
        //
        [Header("[LOG MESSAGES]")]
        public string _NetError = "Something went wrong. Check your internet connection";
        public string _InternetError = "You are not connected to internet";
        public string _HttpError = "Something went wrong. Please try later";
        public string _UnhandledError = "Unhandled error!!";
        public string _Success = "Successful";
    }

}