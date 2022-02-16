using System.Collections.Generic;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QAgentGridWorldConsole : QAgent<QAgentGridWorldConsole, QStateGridWorldConsole>
    {
        private GridWorld _gridWorld;
        
        public QAgentGridWorldConsole(GridWorld gridWorld, List<QStateGridWorldConsole> states,
            List<QAction<QAgentGridWorldConsole, QStateGridWorldConsole>> actions, QStateGridWorldConsole currentState) : base(states, actions, currentState)
        {
            _gridWorld = gridWorld;
        }
        
        public QStateGridWorldConsole GoUp()
        {
            return _gridWorld.GoUp();
        }

        public QStateGridWorldConsole GoDown()
        {
            return _gridWorld.GoDown();
        }

        public QStateGridWorldConsole GoLeft()
        {
            return _gridWorld.GoLeft();
        }

        public QStateGridWorldConsole GoRight()
        {
            return _gridWorld.GoRight();
        }
    }
}