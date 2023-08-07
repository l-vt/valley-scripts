using System;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Quests
{
    public class Quest
    {
        private DateTime m_questStarted;
        public DateTime QuestStarted => m_questStarted;
        [SerializeField] private QuestObject m_questObject;
        [SerializeField] private bool m_isDone = false;
        public bool IsDone => m_isDone;
        public string ShortId => m_questObject.ShortId;
        public string DisplayName => m_questObject.DisplayName.Text;
        public string Description => m_questObject.Description.Text;

        public Quest(QuestObject obj)
        {
            m_questObject = obj;
            m_questStarted = DateTime.Now;
        }

        public void SetDone()
        {
            m_isDone = true;
        }
    }
}
