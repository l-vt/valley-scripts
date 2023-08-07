using Localization;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Q_", menuName = "Quests/Quest", order = 2)]
    public class QuestObject : ScriptableObject
    {
        public string ShortId;
        public Localizable DisplayName = new Localizable();
        public Localizable Description = new Localizable();
    }
}
