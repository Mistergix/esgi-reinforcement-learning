using System;
using System.Collections.Generic;

namespace PGSauce.Games.IaEsgi.Ia
{
    public class QTable
    {
        private Dictionary<QTableCoord, float> _table;

        public QTable(List<QAction> actions, List<QState> states)
        {
            _table = new Dictionary<QTableCoord, float>();

            foreach (var qAction in actions)
            {
                foreach (var qState in states)
                {
                    var coord = new QTableCoord(qAction, qState);
                    _table.Add(coord, 0f);
                }
            }
        }
        
        private struct QTableCoord
        {
            public QAction action;
            public QState state;

            public QTableCoord(QAction action, QState state)
            {
                this.action = action;
                this.state = state;
            }
        }

        public float EvaluateAction(QAction action, QState state)
        {
            var coord = new QTableCoord(action, state);
            if (!_table.ContainsKey(coord))
            {
                throw new ArgumentException(
                    $"Action {action}, State {state} is not in the q table ! Is it initialized ?");
            }

            return _table[coord];
        }
    }
}