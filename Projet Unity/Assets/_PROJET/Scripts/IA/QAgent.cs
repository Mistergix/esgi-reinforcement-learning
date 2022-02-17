using System.Collections.Generic;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAgent<TAgent, TState> : QAgentBase where TAgent : QAgentBase where TState : QState
    {
        private TState _currentState;
        private TState _oldState;

        protected QAgent(List<TState> states, List<QAction<TAgent, TState>> actions, TState currentState)
        {
            _currentState = currentState;
            _oldState = _currentState;
            Actions = actions;
            States = states;
        }

        public TState CurrentState => _currentState;
        public List<QAction<TAgent, TState>> Actions { get; }
        public List<TState> States { get; }

        public TState OldState => _oldState;

        public void TakeAction(QAction<TAgent, TState> action)
        {
            PGDebug.Message($"Current state is {_currentState}").Log();
            _oldState = _currentState;
            _currentState = action.DoAction(this);
            PGDebug.Message($"Current state is after action {_currentState}").Log();
        }

        public abstract float GetCurrentReward();
    }
}