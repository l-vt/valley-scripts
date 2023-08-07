using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    [System.Serializable]
    public class Reactive<T>
    {
        [SerializeField]
        private T m_value;
        public T Value
        {
            set
            {
                m_value = value;
                ForceUpdate();
            }
            get
            {
                return m_value;
            }
        }

        public UnityEvent OnChange = new UnityEvent();

        public Reactive(T initialValue) {
            m_value = initialValue;
        }

        public void ForceUpdate()
        {
            OnChange.Invoke();
        }
    }
}
