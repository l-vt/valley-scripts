using System.Collections.Generic;
using System.Text;
using Entities.Player;
using UnityEngine;

namespace UI.Debug
{
    public class UIDebugVisibleOnGameMode : MonoBehaviour
    {
        [SerializeField] private GameManager.GameMode m_gameMode;
        [SerializeField] private GameObject m_gameObjectToToggle;
        [SerializeField] private bool m_onActiveSet = true;
        
        private void Start()
        {
            GameManager.Instance.CurrentGameMode.OnChange.AddListener(ChangeVisibility);
            m_gameObjectToToggle.SetActive(!m_onActiveSet);
        }

        private void OnDisable()
        {
            GameManager.Instance.CurrentGameMode.OnChange.AddListener(ChangeVisibility);
        }
        

        private void ChangeVisibility()
        {
            GameManager.GameMode gameMode = GameManager.Instance.CurrentGameMode.Value;

            m_gameObjectToToggle.SetActive(gameMode == m_gameMode ? m_onActiveSet : !m_onActiveSet);
        }
    }
}