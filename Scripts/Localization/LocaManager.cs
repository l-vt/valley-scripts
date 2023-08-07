using Enums;
using Localization.ScriptableObjects;
using UnityEngine;

namespace Localization
{
    public class LocaManager : MonoBehaviour
    {
        #region Singleton
        private static LocaManager m_instance;
        public static LocaManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    LocaManager gm = Instantiate(new LocaManager());
                    return gm;
                }
                return m_instance;
            }
        }
        public static LocaManager GetInstance()
        {
            return m_instance;
        }

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            m_instance = this;
        }

        private void OnDestroy()
        {
            m_instance = null;
        }

        #endregion

        [SerializeField] private LanguageFile m_languageFile;
        public LanguageFile LanguageFile => m_languageFile;

        private LanguageFile m_locaInstance;

        private void OnEnable()
        {
            m_locaInstance = Instantiate(m_languageFile);
        }

        public string GetLocalizedString(LocalizationIds id)
        {
            return m_locaInstance.Get(id);
        }
    }
}