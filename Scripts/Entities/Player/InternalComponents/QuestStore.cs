using System;
using System.Collections.Generic;
using Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Player.InternalComponents
{
    [Serializable]
    public class QuestStore
    {
        [HideInInspector] public UnityEvent OnQuestsUpdated = new();
        private List<Quest> m_quests = new();
        public List<Quest> QuestList => m_quests;

        public void Add(Quest quest)
        {
            foreach (var existingQuest in m_quests)
                if (existingQuest.ShortId == quest.ShortId)
                    return;

            m_quests.Add(quest);
            OnQuestsUpdated.Invoke();
        }

        public bool FindAndSetDone(string id)
        {
            foreach (var quest in m_quests)
                if (quest.ShortId == id)
                {
                    quest.SetDone();
                    OnQuestsUpdated.Invoke();
                    return true;
                }

            return false;
        }
    }
}