using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class GameCamera : MonoBehaviour
    {
        [System.Serializable]
        public enum ZoomLevel
        {
            Close = 15,
            Default = 20,
            Wide = 30
        }
        [System.Serializable]
        public struct Target
        {
            public Transform transform;
            public ZoomLevel zoomLevel;
            public Target(Transform t, ZoomLevel z)
            {
                transform = t;
                zoomLevel = z;
            }
        }
        [SerializeField] private List<Target> m_lockTargetPriorities = new List<Target>();
    
        [SerializeField] private UnityEngine.Camera m_camera;
        public UnityEngine.Camera ViewCamera => m_camera;

        private bool m_hasTarget => m_lockTargetPriorities.Count > 0;

        private void Update()
        {
            if (!m_hasTarget) return;

            transform.position = Vector3.Lerp(transform.position, m_lockTargetPriorities[m_lockTargetPriorities.Count - 1].transform.position, Time.deltaTime * 10f);
            Vector3 pos = m_camera.transform.localPosition;
            pos.z = -(float)m_lockTargetPriorities[m_lockTargetPriorities.Count - 1].zoomLevel;
            m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, pos, Time.deltaTime * 1f);
        }

        public void AddTarget(Transform transform, ZoomLevel zoom = ZoomLevel.Default)
        {
            foreach (var tr in m_lockTargetPriorities)
            {
                if (tr.transform == transform) return;
            }

            m_lockTargetPriorities.Add(new Target(transform, zoom));
        }

        public void RemoveTarget(Transform transform)
        {
            foreach (var tr in m_lockTargetPriorities)
            {
                if (tr.transform == transform)
                {
                    m_lockTargetPriorities.Remove(tr);
                    return;
                };
            }
        }
    }
}
