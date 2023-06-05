using System;
using CasualKit.Api;
using CasualKit.Factory;
using Newtonsoft.Json;


namespace CasualKit.Model.Auth
{
    [Serializable]
    public class AuthModel : ModelBase
    {
        [Inject] IWebRequest<AuthModel> _webRequest;
        AuthModel() => CKFactory.Inject(this);

        public void Update() => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl);
        public string Dump() => JsonConvert.SerializeObject(this);
        public static AuthModel Load(string json) => JsonConvert.DeserializeObject<AuthModel>(json);
    }

}