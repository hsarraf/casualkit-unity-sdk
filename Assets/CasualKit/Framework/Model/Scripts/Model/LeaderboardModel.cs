using System;
using CasualKit.Api;
using Newtonsoft.Json;


namespace CasualKit.Model.Leaderboard
{

    [Serializable]
    public class LeaderboardModel : ModelBase
    {
        public string Dump() => JsonConvert.SerializeObject(this);
        public static LeaderboardModel Load(string json) => JsonConvert.DeserializeObject<LeaderboardModel>(json);
    }

}