using UnityEngine;
using Utils.StateMachine;

namespace Entities.NPC.States.Searcher
{
    public class StateLookAround : IStateMachineState<NPC>
    {
        private float m_timer;
        
        public void OnEnterState(StateMachine<NPC> stateMachine)
        {
            m_timer = 0;
        }

        public void OnLeaveState(StateMachine<NPC> stateMachine)
        {
        }

        public void OnUpdateState(StateMachine<NPC> stateMachine)
        {
            m_timer += Time.deltaTime;
            stateMachine.Owner.VisualsTransform.Rotate(Vector3.up, 2f * Time.deltaTime);

            if (m_timer <= 2f) return;
            
            if (stateMachine.Owner.Trigger.ObjectCount > 0)
            {
                stateMachine.SwitchToState("Chasing");
                return;
            }
            
            if (m_timer <= 5f) return;
            
            stateMachine.SwitchToState("Walking");
        }

        public string StateName()
        {
            return "LookAround";
        }
    }
}