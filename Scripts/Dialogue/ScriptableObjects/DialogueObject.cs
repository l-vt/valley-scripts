using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DL_", menuName = "Dialogue/Dialogue", order = 3)]
    public class DialogueObject : ScriptableObject
    {
        [System.Serializable]
        public struct Sentence
        {
            [TextArea]
            public string Text;
        }

        [SerializeField] private List<Sentence> m_sentences;
        public List<Sentence> Sentences => m_sentences;
    }
}
