using Entities.NPC.States.Static;
using Utils.StateMachine;

namespace Entities.NPC.Types
{
    public class StaticNPC : NPC
    {
        protected override void InitializeStates(ref StateMachine<NPC> stateMachine)
        {
            stateMachine = new StateMachine<NPC>(this, new StateIdle());
            stateMachine.SwitchToState("Idle");
        }
    }
}