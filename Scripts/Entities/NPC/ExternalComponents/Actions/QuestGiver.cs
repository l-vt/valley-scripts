using System;
using System.Collections.Generic;
using Quests;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Entities.NPC.ExternalComponents.Actions
{
    public class QuestGiver : Action
    {
        [SerializeField] private List<QuestObject> m_questObjects = new();

        public override void PerformAction(Player.Player player, Func<Player.Player, int> next)
        {
            foreach (var quest in m_questObjects)
            {
                player.QuestStore.Add(new Quest(quest));
            }
            next(player);
        }
    }
}