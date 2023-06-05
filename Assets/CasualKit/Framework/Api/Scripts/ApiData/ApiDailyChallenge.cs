using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.DailyChallenge;
using CasualKit.Model.Leaderboard;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.DailyChallenge
{

    public class ApiDailyChallnege : IApiModel<DailyChallengeModel>
    {
        [Inject] IDataModel _model;
        [Inject] IWebRequest<DailyChallengeModel> _webRequest;
        ApiDailyChallnege() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<DailyChallengeModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<DailyChallengeModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (sd) => { _model.PlayerData.dailyChallenge = sd.payload; onSuccess?.Invoke(sd); }, onFail);
    }

}