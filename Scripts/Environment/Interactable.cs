using Entities.Player;
using Localization;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [System.Serializable]
    public class ExecuteEvent : UnityEvent<Player>
    {
    }
    public class Interactable : MonoBehaviour
    {
        public ExecuteEvent OnRequestExecute = new ExecuteEvent();

        [SerializeField] private Localizable m_displayName = new Localizable();
        public string DisplayName => m_displayName.Text;

        public void Execute(Player player)
        {
            OnRequestExecute.Invoke(player);
        }
    }
}
