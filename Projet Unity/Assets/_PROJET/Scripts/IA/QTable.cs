using System;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;

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
            public readonly QAction<TAgent, TState> action;
            public readonly QState state;

            public QTableCoord(QAction<TAgent, TState> action, QState state)
            {
                this.action = action;
                this.state = state;
            }

            public override string ToString()
            {
                return $"[{action}, {state}]";
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public bool Equals(QTableCoord other)
            {
                return Equals(action, other.action) && Equals(state, other.state);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((action != null ? action.GetHashCode() : 0) * 397) ^ (state != null ? state.GetHashCode() : 0);
                }
            }
        }

        public float EvaluateAction(QAction<TAgent, TState> action, QState state)
        {
            var coord = GetQTableCoord(action, state);

            return _table[coord];
        }

        private QTableCoord GetQTableCoord(QAction<TAgent, TState> action, QState state)
        {
            var coord = new QTableCoord(action, state);
            if (!_table.ContainsKey(coord))
            {
                throw new ArgumentException(
                    $"Action {action}, State {state} is not in the q table ! Is it initialized ?");
            }

            return coord;
        }

        public void UpdateValue(TState agentOldState, QAction<TAgent, TState> qAction, float newValue)
        {
            var coord = GetQTableCoord(qAction, agentOldState);
            PGDebug.Message($"Old value in table at {coord} is {_table[coord]}").Log();
            _table[coord] = newValue;
            PGDebug.Message($"New value in table at {coord} is {_table[coord]}").Log();
        }
    }
}