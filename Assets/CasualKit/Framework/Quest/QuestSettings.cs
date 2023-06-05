using CasualKit.Model.Quest;
using UnityEngine;


namespace CasualKit.Quest.Settings
{

    [CreateAssetMenu(fileName = "QuestSettings", menuName = "CasualKit/QuestSettings")]
    public class QuestSettings : ScriptableObject
    {
        public QuestModel[] _Quests;
    }

}