using System.Collections.Generic;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAgent<TAgent, TState> : QAgentBase where TAgent : QAgentBase where TState : QState
    {
        private TState _currentState;

        protected QAgent(List<TState> states, List<QAction<TAgent, TState>> actions, TState currentState)
        {
            _currentState = currentState;
            Actions = actions;
            States = states;
        }

        public TState CurrentState => _currentState;
        public List<QAction<TAgent, TState>> Actions { get; }
        public List<TState> States { get; }

        public void TakeAction(QAction<TAgent, TState> action)
        {
            PGDebug.Message($"Current state is {_currentState}").Log();
            _currentState = action.DoAction(this);
            PGDebug.Message($"Current state is after action {_currentState}").Log();
        }
    }
}