using System;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QActionGridWorldConsole : QAction<QAgentGridWorldConsole, QStateGridWorldConsole>
    {
        public QActionGridWorldConsole(Func<QAgentGridWorldConsole, QStateGridWorldConsole> agent) : base(agent)
        {
        }
    }
}