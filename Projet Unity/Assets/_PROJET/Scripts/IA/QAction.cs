using System;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAction<TAgent, TState> : QActionBase where TAgent : QAgentBase where TState: QState
    {
        private Func<TAgent, TState> _action;

        public QAction(Func<TAgent, TState> action)
        {
            _action = action;
        }

        public TState DoAction(QAgent<TAgent, TState> qAgent)
        {
            return _action(qAgent as TAgent);
        }
    }
}