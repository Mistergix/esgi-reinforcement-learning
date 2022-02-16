using System.Collections.Generic;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QAgentGridWorldConsole : QAgent<QAgentGridWorldConsole>
    {
        private GridWorld _gridWorld;
        
        public QAgentGridWorldConsole(GridWorld gridWorld, List<QState> states,
            List<QAction<QAgentGridWorldConsole>> actions, QState currentState) : base(states, actions, currentState)
        {
            _gridWorld = gridWorld;
        }
        
        public QState GoUp()
        {
            return _gridWorld.GoUp();
        }

        public QState GoDown()
        {
            return _gridWorld.GoDown();
        }

        public QState GoLeft()
        {
            return _gridWorld.GoLeft();
        }

        public QState GoRight()
        {
            return _gridWorld.GoRight();
        }
    }
}