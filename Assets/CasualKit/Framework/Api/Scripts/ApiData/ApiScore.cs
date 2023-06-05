using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.Score
{

    public class ApiScore: IApiModel<ScoreModel>
    {
        [Inject] IDataModel _model;
        [Inject] IWebRequest<ScoreModel> _webRequest;
        ApiScore() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<ScoreModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<ScoreModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (sd) => { _model.PlayerData.score = sd.payload; onSuccess?.Invoke(sd); }, onFail);
    }

}