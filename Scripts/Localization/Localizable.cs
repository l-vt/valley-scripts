using Enums;
using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class Localizable
    {
        [SerializeField] private LocalizationIds m_locaID;
        public string Text => LocaManager.Instance.GetLocalizedString(m_locaID);
    }
}
