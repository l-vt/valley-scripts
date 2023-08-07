using Entities.NPC.States;
using Entities.NPC.States.Searcher;
using Utils.StateMachine;

namespace Entities.NPC.Types
{
    public class SearcherNPC : NPC
    {
        protected override void InitializeStates(ref StateMachine<NPC> stateMachine)
        {
            stateMachine = new StateMachine<NPC>(
                this, 
                new StateLookAround(),
                new StateWalking(),
                new StateChasing()
            );
            stateMachine.SwitchToState("LookAround");
        }
    }
}