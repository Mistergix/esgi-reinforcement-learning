using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class QAgentSokoban : QAgent<QAgentSokoban, QStateSokoban>
    {
        private ISokobanAi _sokoban;
        public QAgentSokoban(ISokobanAi sokoban, List<QStateSokoban> states, List<QAction<QAgentSokoban, QStateSokoban>> actions, QStateSokoban currentState) : base(states, actions, currentState)
        {
            _sokoban = sokoban;
        }

        public QStateSokoban GoUp()
        {
            return _sokoban.GoUp();
        }

        public QStateSokoban GoDown()
        {
            return _sokoban.GoDown();
        }

        public QStateSokoban GoLeft()
        {
            return _sokoban.GoLeft();
        }

        public QStateSokoban GoRight()
        {
            return _sokoban.GoRight();
        }

        public override float GetCurrentReward(QStateSokoban @from, QStateSokoban to, QAction<QAgentSokoban, QStateSokoban> policyAction)
        {
            return _sokoban.GetTileValue(to.Coords);
        }
    }
}
