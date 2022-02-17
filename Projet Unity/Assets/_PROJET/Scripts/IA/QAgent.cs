using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.GridWorldConsole;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAgent<TAgent, TState> : QAgentBase where TAgent : QAgentBase where TState : QState
    {
        private TState _oldState;

        protected QAgent(List<TState> states, List<QAction<TAgent, TState>> actions, TState currentState)
        {
            CurrentState = currentState;
            _oldState = CurrentState;
            Actions = actions;
            States = states;
        }

        public TState CurrentState { get; set; }

        public List<QAction<TAgent, TState>> Actions { get; }
        public List<TState> States { get; }

        public TState OldState => _oldState;

        public void TakeAction(QAction<TAgent, TState> action)
        {
            PGDebug.Message($"Current state is {CurrentState}").Log();
            _oldState = CurrentState;
            CurrentState = action.DoAction(this);
            PGDebug.Message($"Current state is after action {CurrentState}").Log();
        }

        public abstract float GetCurrentReward();
    }
}