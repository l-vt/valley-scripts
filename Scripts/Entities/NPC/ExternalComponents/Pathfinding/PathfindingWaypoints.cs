using System;
using UnityEngine;

namespace Entities.NPC.ExternalComponents.Pathfinding
{
    public class PathfindingWaypoints : MonoBehaviour
    {
        [SerializeField] private Vector3[] m_wayPoints;
        public Vector3[] WayPoints => m_wayPoints;

        private void OnDrawGizmosSelected()
        {
            foreach (var pos in m_wayPoints)
            {
                Gizmos.DrawSphere(pos, .1f);
                Gizmos.DrawLine(pos - Vector3.forward, pos + Vector3.forward);
                Gizmos.DrawLine(pos - Vector3.up, pos + Vector3.up);
                Gizmos.DrawLine(pos - Vector3.right, pos + Vector3.right);
            }
        }
    }
}