using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class QStateSokoban : QState
    {
        private Coords _coords;
        public QStateSokoban(Coords coords)
        {
            _coords = coords;
        }

        public Coords Coords => _coords;

        public override string ToString()
        {
            return $"[STATE SOKOBAN {_coords}]";
        }
    }
}
