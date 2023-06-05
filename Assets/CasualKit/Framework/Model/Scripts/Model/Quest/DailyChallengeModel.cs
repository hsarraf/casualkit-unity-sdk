using System;
using CasualKit.Api;
using CasualKit.Model.Quest;
using Newtonsoft.Json;


namespace CasualKit.Model.DailyChallenge
{

    [Serializable]
    public class DailyChallengeModel : QuestModel
    {
        public static DailyChallengeModel Load(string json) => JsonConvert.DeserializeObject<DailyChallengeModel>(json);
    }

}