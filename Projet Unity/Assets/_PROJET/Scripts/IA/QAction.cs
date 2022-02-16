using System;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAction<TAgent> : QActionBase where TAgent : QAgentBase
    {
        private Func<TAgent, QState> _action;

        public QAction(Func<TAgent, QState> action)
        {
            _action = action;
        }

        public QState DoAction(QAgent<TAgent> qAgent)
        {
            return _action(qAgent as TAgent);
        }
    }
}