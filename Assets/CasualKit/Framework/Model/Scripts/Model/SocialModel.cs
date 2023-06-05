using System;
using Newtonsoft.Json;


namespace CasualKit.Model.Profile
{

    [Serializable]
    public class SocialModel : ModelBase
    {
        public string socialId;
        public string[] pendingFriends;
        public string[] friends;
        public TeamModel[] teams;

        public string Dump() => JsonConvert.SerializeObject(this);
        public static SocialModel Load(string json) => JsonConvert.DeserializeObject<SocialModel>(json);
    }

    [Serializable]
    public class TeamModel
    {
        public string teamId;
        public string[] people;
        public string Dump() => JsonConvert.SerializeObject(this);
        public static TeamModel Load(string json) => JsonConvert.DeserializeObject<TeamModel>(json);
    }

}