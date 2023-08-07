using System.Collections.Generic;

namespace Utils.StateMachine
{
    public class StateMachine<T>
    {
        private Dictionary<string, IStateMachineState<T>> m_states;
        private string m_currentStateString;
        private IStateMachineState<T> m_currentState => m_states[m_currentStateString];

        private T m_objectToPass;
        public T Owner => m_objectToPass;

        private string m_initialState;


        public StateMachine(T objectToPass, params IStateMachineState<T>[] states) {
            m_states = new Dictionary<string, IStateMachineState<T>>();

            foreach (var state in states)
            {
                m_states.Add(state.StateName(), state);
            }

            m_objectToPass = objectToPass;
            // Switching to initial state immediately might result in some data not being available while entering
            // the first state. Hence, users must call SwitchToFirstState during Start
            m_initialState = states[0].StateName();
        }

        public void SwitchToFirstState()
        {
            SwitchToState(m_initialState);
        }

        public void RegisterState(string key, IStateMachineState<T> value)
        {
            m_states.Add(key, value);
        }

        public void SwitchToState(string name)
        {
            if (m_currentStateString != null)
            {
                m_currentState.OnLeaveState(this);
            }
            m_currentStateString = name;

            m_currentState.OnEnterState(this);
        }

        public void Update()
        {
            CallUpdateState();
        }

        protected void CallEnterState()
        {
            if (m_currentStateString == null) return;

            m_currentState.OnEnterState(this);
        }

        protected void CallLeaveState()
        {
            if (m_currentStateString == null) return;

            m_currentState.OnLeaveState(this);
        }

        protected void CallUpdateState()
        {
            if (m_currentStateString == null) return;

            m_currentState.OnUpdateState(this);
        }
    }
}
