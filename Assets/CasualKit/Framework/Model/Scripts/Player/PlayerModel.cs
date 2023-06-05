using System;
using CasualKit.Model.DailyChallenge;
using CasualKit.Model.Leaderboard;
using CasualKit.Model.Payment;
using CasualKit.Model.Profile;
using Newtonsoft.Json;


namespace CasualKit.Model.Player
{

    [Serializable]
    public class PlayerModel : IPlayerModel
    {
        public string userId;

        public string username;

        public ProfileModel profile;

        public SocialModel social { get; set; }

        public PaymentModel payment;

        public ScoreModel score;

        public DailyChallengeModel dailyChallenge;

        public LeaderboardModel leaderboard;


        public void Update(PlayerModel pd) => (userId, username, profile, social, payment, score, dailyChallenge) =
                                              (pd.userId, pd.username, pd.profile, pd.social, pd.payment, pd.score, pd.dailyChallenge);
        //public void Push() => WebRequest<PlayerModel>.POSTJSON(this, CKSettings.Auth.RegisterUrl);
        //public event Action<WebResponse<string>> OnPushed;
        //public void Fetch() => WebRequest<PlayerModel>.POSTJSON(this, CKSettings.Auth.RegisterUrl);
        public string Dump() => JsonConvert.SerializeObject(this);
        public static PlayerModel Load(string json) => JsonConvert.DeserializeObject<PlayerModel>(json);
    }

}