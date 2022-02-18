using System.Collections.Generic;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QAgentGridWorldConsole : QAgent<QAgentGridWorldConsole, QStateGridWorldConsole>
    {
        private IGridWorldAi _gridWorld;
        
        public QAgentGridWorldConsole(IGridWorldAi gridWorld, List<QStateGridWorldConsole> states,
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

        public override float GetCurrentReward(QStateGridWorldConsole @from, QStateGridWorldConsole to, QAction<QAgentGridWorldConsole, QStateGridWorldConsole> policyAction)
        {
           return _gridWorld.GetTileValue(to.Coords);
        }
    }
}