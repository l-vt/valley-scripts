using System.Collections.Generic;
using System.Text;
using Entities.Player;
using Quests;
using UnityEngine;

namespace UI.Debug
{
    public class UIDebugQuests : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private TMPro.TMP_Text m_text;
        private void OnEnable()
        {
            m_player.QuestStore.OnQuestsUpdated.AddListener(SetDisplayText);
        }
        private void OnDisable()
        {
            m_player.QuestStore.OnQuestsUpdated.RemoveListener(SetDisplayText);
        }

        private void SetText(string action)
        {
            m_text.text = action;
        }

        private void SetDisplayText()
        {
            StringBuilder str = new StringBuilder();
            List<Quest> quests = m_player.QuestStore.QuestList;
        
            foreach (var q in quests)
            {
                str.AppendLine(q.DisplayName);
                if (q.IsDone)
                {
                    str.Append(" (DONE)");
                }
                str.AppendLine(q.Description);
            }
            SetText(str.ToString());
        }
    }
}
