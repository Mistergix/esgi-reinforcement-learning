using System.Collections.Generic;

namespace PGSauce.Games.IaEsgi.Ia
{
    public class QPolicy<TAgent, TState> where TAgent : QAgentBase where TState : QState
    {
        public QAction<TAgent, TState> Action { get; set; }
    }
}