using System;
using System.Collections.Generic;

namespace PGSauce.Games.IaEsgi.Ia
{
    public class QTable<TAgent, TState> where TAgent : QAgentBase where TState : QState
    {
        private Dictionary<QTableCoord, float> _table;

        public QTable(List<QAction<TAgent, TState>> actions, List<TState> states)
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
            public QAction<TAgent, TState> action;
            public QState state;

            public QTableCoord(QAction<TAgent, TState> action, QState state)
            {
                this.action = action;
                this.state = state;
            }
        }

        public float EvaluateAction(QAction<TAgent, TState> action, QState state)
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