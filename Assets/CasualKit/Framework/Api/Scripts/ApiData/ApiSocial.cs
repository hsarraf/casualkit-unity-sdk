using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.Social
{

    public class ApiProfile : IApiModel<SocialModel>
    {
        [Inject] IDataModel _model;
        [Inject] IWebRequest<SocialModel> _webRequest;
        ApiProfile() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<SocialModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<SocialModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (sd) => { _model.PlayerData.social = sd.payload; onSuccess?.Invoke(sd); }, onFail);
    }

}