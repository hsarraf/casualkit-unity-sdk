using CasualKit.Model.Quest;
using Newtonsoft.Json;
using System;


namespace CasualKit.Model.Achievement
{

    [Serializable]
    public class AchievementContent
    {
        public QuestModel[] achievements;
        public string Dump() => JsonConvert.SerializeObject(this);
        public static AchievementContent Load(string json) => JsonConvert.DeserializeObject<AchievementContent>(json);
    }

    [Serializable]
    public class AchievementModel : ModelBase
    {
        public int[] doneList;
        public string Dump() => JsonConvert.SerializeObject(this);
        public static AchievementModel Load(string json) => JsonConvert.DeserializeObject<AchievementModel>(json);
    }

}