using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [System.Serializable]
    public class TriggerBoxEvent<T> : UnityEvent<T> { }
    public class TriggerBox<T> : MonoBehaviour
    {
        [SerializeField] private bool m_useLayerMask = false;
        [SerializeField] private LayerMask m_layerMask;
        [Space]

        [SerializeField] [ReadOnly] private List<T> m_inTrigger = new List<T>();
        public List<T> ObjectsInTrigger => m_inTrigger;
        public int ObjectCount => m_inTrigger.Count;

        public TriggerBoxEvent<T> OnEnter = new TriggerBoxEvent<T>();
        public TriggerBoxEvent<T> OnExit = new TriggerBoxEvent<T>();

        private void OnEnable()
        {
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_useLayerMask)
            {
                if (m_layerMask != (m_layerMask | (1 << other.gameObject.layer))) {
                    return;
                }
            }

            GameObject go = other.gameObject;
            
            if (go == null) return;
            T component = go.GetComponent<T>();

            if (component == null) return;
            
            if (!m_inTrigger.Contains(component))
            {
                m_inTrigger.Add(component);
                OnEnter.Invoke(component);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            GameObject go = other.gameObject;
            if (go == null) return;
            
            T component = go.GetComponent<T>();
            if (component == null) return;

            if (m_inTrigger.Contains(component) )
            {
                m_inTrigger.Remove(component);
                OnExit.Invoke(component);
            }
            
        }
    }
}