using System;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAction<TAgent, TState> : QActionBase where TAgent : QAgentBase where TState: QState
    {
        private Func<TAgent, TState> _action;
        private string _name;

        public QAction(Func<TAgent, TState> action, string name)
        {
            _action = action;
            _name = name;
        }

        public string Name => _name;

        public override string ToString()
        {
            return Name;
        }

        public TState DoAction(QAgent<TAgent, TState> qAgent)
        {
            PGDebug.Message($"Doing Action {Name}").Log();
            return _action(qAgent as TAgent);
        }
    }
}