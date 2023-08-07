using System.Collections.Generic;
using Enums;
using UnityEngine;
using Utils;

namespace Localization.ScriptableObjects
{
    [System.Serializable]
    public class LocaEntry
    {
        public string ID;
        [TextArea]
        public string Translated;
        public int ValId;
    }

    [CreateAssetMenu(fileName = "LGN_", menuName = "Localization/LanguageFile", order = 1)]
    public class LanguageFile : ScriptableObject
    {
        public List<LocaEntry> Entries = new List<LocaEntry>();

        [HideInInspector][SerializeField] private int IdCount = 0;

        public string Get(LocalizationIds locaId)
        {
            foreach (var entry in Entries)
            {
                if (entry.ID == locaId.ToString())
                {
                    return entry.Translated;
                }
            }
            return "Missing string";
        }

        public void OnValidate()
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                LocaEntry entry = Entries[i];
                Entries[i].ID.Replace(".", "");
                if (entry.ValId == 0)
                {
                    Entries[i].ValId = IdCount;
                    IdCount++;
                }
            }
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button("Generate IDs")]
        public void GenerateIDs() {
            GenerateEnum.Generate("LocalizationIds", Entries.ToArray());
        }
#endif
    }
}