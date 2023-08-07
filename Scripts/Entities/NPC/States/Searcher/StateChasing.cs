using UnityEngine;
using Utils;
using Utils.StateMachine;

namespace Entities.NPC.States.Searcher
{
    public class StateChasing : IStateMachineState<NPC>
    {
        private Vector3 m_lastKnownPlayerPosition = Vector3.zero;
        
        private readonly float m_updateInterval = 0.5f;
        private float m_updateIntervalCounter = 0f;

        private Entity m_chasedEntity = null;
        private float m_targetRadius = .75f;
        
        public void OnEnterState(StateMachine<NPC> stateMachine)
        {
            m_lastKnownPlayerPosition = stateMachine.Owner.transform.position;
            
            m_chasedEntity = stateMachine.Owner.Trigger.ObjectsInTrigger[0];
            m_targetRadius = m_chasedEntity.Radius + stateMachine.Owner.Radius;

            UpdateLocation(stateMachine);
            
            stateMachine.Owner.MovementSpeed.SetSprinting(true);
        }

        public void OnLeaveState(StateMachine<NPC> stateMachine)
        {
            stateMachine.Owner.MovementSpeed.SetSprinting(false);
        }

        public void OnUpdateState(StateMachine<NPC> stateMachine)
        {
            m_updateIntervalCounter += Time.deltaTime;
            if (m_updateIntervalCounter >= m_updateInterval)
            {
                UpdateLocation(stateMachine);
            }

            if (!stateMachine.Owner.Pathfinding.IsAtTarget)
            {
                return;
            }
            
            float distanceToPlayer = Vector3.Distance(
                m_chasedEntity.transform.position,
                stateMachine.Owner.transform.position
            );
            
            if (distanceToPlayer <= m_targetRadius)
            {
                GameManager.Instance.SetGameOver();
            }
            else
            {
                stateMachine.SwitchToState("LookAround");
            }
        }

        private void UpdateLocation(StateMachine<NPC> stateMachine)
        {
            if (stateMachine.Owner.Trigger.ObjectCount <= 0)
            {
                return;
            }

            bool canSee = ChasedEntityInSight(
                stateMachine.Owner.AbsoluteEyePosition,
                m_chasedEntity.AbsoluteEyePosition
            );
            
            if (!canSee)
            {
                return;
            }

            m_lastKnownPlayerPosition = m_chasedEntity.transform.position;

            stateMachine.Owner.Pathfinding.SetTarget(m_lastKnownPlayerPosition, m_targetRadius);
        }

        private bool ChasedEntityInSight(Vector3 a, Vector3 b)
        {
            return (!Physics.Linecast(a, b));
        }

        public string StateName()
        {
            return "Chasing";
        }
    }
}