using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.Leaderboard;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.Leaderboard
{

    public class ApiLeaderboard : IApiModel<LeaderboardModel>
    {
        [Inject] IDataModel _model;
        [Inject] IWebRequest<LeaderboardModel> _webRequest;
        ApiLeaderboard() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<LeaderboardModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<LeaderboardModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (sd) => { _model.PlayerData.leaderboard = sd.payload; onSuccess?.Invoke(sd); }, onFail);
    }

}