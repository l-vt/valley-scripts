using System;
using System.Collections.Generic;
using UnityEngine;
using Action = Entities.NPC.ExternalComponents.Actions.Action;

namespace Entities.NPC.InternalComponents
{
    [Serializable]
    public class Interacting
    {
        [SerializeField] private List<Action> m_actions = new();
        private int m_callbackIndex;

        public void PerformAction(Player.Player player)
        {
            m_callbackIndex = -1;
            RunAction(player);
        }

        private int RunAction(Player.Player player)
        {
            m_callbackIndex++;
            if (m_callbackIndex >= m_actions.Count)
            {
                GameManager.Instance.SetGameMode(GameManager.GameMode.Walking);
                return 0;
            }

            m_actions[m_callbackIndex].PerformAction(player, RunAction);
            return 1;
        }

    }
}