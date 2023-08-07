using Entities.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class FulfillQuest : MonoBehaviour
    {
        // #todo: smells weird; rename
        [SerializeField] private string m_questId = "";
        public string QuestId => m_questId;
        public UnityEvent OnQuestFulfilled = new UnityEvent();

        public void SetQuestDone(Player player)
        {
            bool didFulfill = player.QuestStore.FindAndSetDone(m_questId);
            if (didFulfill)
            {
                OnQuestFulfilled.Invoke();
            }
        }
    }
}
