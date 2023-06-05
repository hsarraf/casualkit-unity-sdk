using System;
using Newtonsoft.Json;


namespace CasualKit.Model.Profile
{

    [Serializable]
    public class ScoreModel : ModelBase
    {
        public int coin;
        public int gem;
        public int cup;
        public int score;
        public int level;
        public int rank;
        public int xp;
        public int fan;
        public string Dump() => JsonConvert.SerializeObject(this);
        public static ScoreModel Load(string json) => JsonConvert.DeserializeObject<ScoreModel>(json);
    }

}