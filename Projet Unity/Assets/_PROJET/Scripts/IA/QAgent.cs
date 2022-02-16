using System.Collections.Generic;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAgent<TAgent> : QAgentBase where TAgent : QAgentBase
    {
        private QState _currentState;

        protected QAgent(List<QState> states, List<QAction<TAgent>> actions, QState currentState)
        {
            _currentState = currentState;
            Actions = actions;
            States = states;
        }

        public QState CurrentState => _currentState;
        public List<QAction<TAgent>> Actions { get; }
        public List<QState> States { get; }

        public void TakeAction(QAction<TAgent> action)
        {
            _currentState = action.DoAction(this);
        }
    }
}