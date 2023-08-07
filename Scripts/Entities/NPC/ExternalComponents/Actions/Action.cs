using System;
using UnityEngine;

namespace Entities.NPC.ExternalComponents.Actions
{
    public abstract class Action : MonoBehaviour
    {
        public abstract void PerformAction(Player.Player player, Func<Player.Player, int> next);
    }
}