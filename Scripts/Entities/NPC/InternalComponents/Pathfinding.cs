using System;
using NaughtyAttributes;
using UnityEngine;

namespace Entities.NPC.InternalComponents
{
    [Serializable]
    public class Pathfinding
    {
        private readonly NPC m_npc;
        
        [SerializeField]
        private bool m_isAtTarget;
        public bool IsAtTarget => m_isAtTarget;
        private Vector3 m_targetPosition;
        private float m_goalDistanceThreshold = 0.2f;

        public Pathfinding(NPC npc)
        {
            m_npc = npc;
            m_isAtTarget = true;
            m_targetPosition = m_npc.transform.position;
        }

        public void SetTarget(Vector3 targetPosition, float radius = 0.2f)
        {
            m_targetPosition = targetPosition;
            m_goalDistanceThreshold = radius;
            m_isAtTarget = false;
        }

        public void Update()
        {
            if (m_isAtTarget)
            {
                return;
            }
            
            float distance = Vector3.Distance(m_targetPosition, m_npc.transform.position);
            if (distance <= m_goalDistanceThreshold)
            {
                if (!m_isAtTarget)
                {
                    ReachTarget();
                }
                return;
            }
            
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            // #todo: A* pathfinding, or something more complicated dedicated
            Vector3 nonNormalizedDirection = m_targetPosition - m_npc.transform.position;
            
            m_npc.Rigidbody.ApplyMovement(nonNormalizedDirection);
        }

        private void ReachTarget()
        {
            m_isAtTarget = true;
        }

        public void Destroy()
        {
        }
    }
}