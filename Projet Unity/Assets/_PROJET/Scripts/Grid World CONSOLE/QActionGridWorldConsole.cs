using System;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QActionGridWorldConsole : QAction<QAgentGridWorldConsole>
    {
        public QActionGridWorldConsole(Func<QAgentGridWorldConsole, QState> agent) : base(agent)
        {
        }
    }
}