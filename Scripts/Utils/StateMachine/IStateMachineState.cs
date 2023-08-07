namespace Utils.StateMachine
{
    public interface IStateMachineState<T>
    {
        public string StateName();
        public void OnEnterState(StateMachine<T> stateMachine);
        public void OnLeaveState(StateMachine<T> stateMachine);
        public void OnUpdateState(StateMachine<T> stateMachine);
    }
}
