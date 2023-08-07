using Entities.Player;
using UnityEngine;

namespace Environment
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private Transform m_targetPosition;

        public void TeleportPlayer(Player player)
        {
            player.transform.position = m_targetPosition.position;
            player.transform.rotation = m_targetPosition.rotation;
        }
    }
}
