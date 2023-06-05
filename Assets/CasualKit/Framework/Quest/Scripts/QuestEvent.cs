using CasualKit.Model.Quest;


namespace CasualKit.Quest {

    public enum QuestType
    {
        DailiChallenge, Mission, Achievement
    }

    public class QuestEvent
    {
        public QuestEvent(QuestModel quest, QuestType type) => (_quest, _type) = (quest, type);

        public QuestType _type;
        public QuestModel _quest;
        public bool Check(int current) => current >= _quest.target;
    }

}