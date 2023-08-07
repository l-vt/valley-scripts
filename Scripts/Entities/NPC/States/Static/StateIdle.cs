using UnityEngine;
using Utils.StateMachine;

namespace Entities.NPC.States.Static
{
    public class StateIdle : IStateMachineState<NPC>
    {

        public void OnEnterState(StateMachine<NPC> stateMachine)
        {

        }

        public void OnLeaveState(StateMachine<NPC> stateMachine)
        {
        }

        public void OnUpdateState(StateMachine<NPC> stateMachine)
        {

        }

        public string StateName()
        {
            return "Idle";
        }
    }
}