using Newtonsoft.Json;
using System;


namespace CasualKit.Model.Quest
{

    public enum TargetType
    {
        Win,
        Fan,
        Power
    }

    public enum RewardType
    {
        Fan,
        Coin,
        Gem
    }

    [Serializable]
    public class QuestModel
    {
        public string name;
        public int index;
        public string description;
        public string icon;

        public TargetType targetType;
        public int target;

        public RewardType rewardType;
        public int reward;

        public string Dump() => JsonConvert.SerializeObject(this);
    }

}