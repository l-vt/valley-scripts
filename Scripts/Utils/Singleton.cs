using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T m_instance;
        public static T Instance => m_instance;

        private void OnEnable()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
            } else
            {
                Destroy(gameObject);
                return;
            }
        }

    }
}
