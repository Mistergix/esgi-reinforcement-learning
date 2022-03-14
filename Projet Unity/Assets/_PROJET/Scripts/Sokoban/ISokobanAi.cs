using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public interface ISokobanAi
    {
        public QStateSokoban GoUp();

        public QStateSokoban GoDown();

        public QStateSokoban GoLeft();

        public QStateSokoban GoRight();

        float GetTileValue(Coords coords);
        QStateSokoban PushUp();
        QStateSokoban PushDown();
        QStateSokoban PushLeft();
        QStateSokoban PushRight();
    }
}
