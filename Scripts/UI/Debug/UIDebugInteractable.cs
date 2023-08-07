using Entities.Player;
using Localization;
using UnityEngine;

namespace UI.Debug
{
    public class UIDebugInteractable : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private TMPro.TMP_Text m_text;
        [SerializeField] private Localizable m_initialText = new Localizable();
        [SerializeField] private GameObject m_wrapperToToggleVisibility;
        private string m_nonText = "/";

        private void Start()
        {
            m_player.CurrentInteractable.OnChange.AddListener(SetDisplayText);

            SetDisplayText();
        }
        private void OnDisable()
        {
            m_player.CurrentInteractable.OnChange.RemoveListener(SetDisplayText);
        }

        private void SetText(string action)
        {
            m_text.text = m_initialText.Text.Replace("$1", action);
        }

        private void SetDisplayText()
        {
            bool hasInteractable = m_player.CurrentInteractable.Value != null;
            
            m_wrapperToToggleVisibility.SetActive(hasInteractable);
            if (!hasInteractable)
            {
                SetText(m_nonText);
                return;
            }
            SetText(m_player.CurrentInteractable.Value.DisplayName);
        }

    }
}
