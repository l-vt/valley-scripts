using System;
using Entities.NPC.ExternalComponents.Pathfinding;
using UnityEngine;
using Utils.StateMachine;

namespace Entities.NPC.States.Searcher
{
    public class StateWalking : IStateMachineState<NPC>
    {
        private int m_currentWayPointIndex = 0;
        private PathfindingWaypoints m_wayPoints;

        public void OnEnterState(StateMachine<NPC> stateMachine)
        {
            // costly
            m_wayPoints = stateMachine.Owner.GetComponent<PathfindingWaypoints>();
            if (m_wayPoints == null)
            {
                throw new Exception(
                    $"Searcher is {stateMachine.Owner.DisplayName} expected to havePathfindingWaypoints component attached"
                );
            }
            
            // #todo: pick from list of predefined paths
            Vector3 randomLocation = m_wayPoints.WayPoints[m_currentWayPointIndex];
            
            stateMachine.Owner.Pathfinding.SetTarget(randomLocation);
        }

        public void OnLeaveState(StateMachine<NPC> stateMachine)
        {
            m_currentWayPointIndex = (m_currentWayPointIndex + 1) % m_wayPoints.WayPoints.Length;
        }

        public void OnUpdateState(StateMachine<NPC> stateMachine)
        {
            if (stateMachine.Owner.Pathfinding.IsAtTarget)
            {
                stateMachine.SwitchToState("LookAround");
                return;
            }
            
            if (stateMachine.Owner.Trigger.ObjectCount > 0)
            {
                stateMachine.SwitchToState("Chasing");
                return;
            }
        }

        public string StateName()
        {
            return "Walking";
        }
    }
}