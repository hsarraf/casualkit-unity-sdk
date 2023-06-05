using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.Profile
{

    public class ApiProfile : IApiModel<ProfileModel>
    {

        [Inject] IDataModel _model;
        [Inject] IWebRequest<ProfileModel> _webRequest;
        ApiProfile() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<ProfileModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<ProfileModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (pd) => { _model.PlayerData.profile = pd.payload; onSuccess?.Invoke(pd); }, onFail);

    }

}