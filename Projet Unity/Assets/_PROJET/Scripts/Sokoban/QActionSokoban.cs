using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;
using System;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class QActionSokoban : QAction<QAgentSokoban, QStateSokoban>
    {
        public QActionSokoban(Func<QAgentSokoban, QStateSokoban> action, string name) : base(action, name)
        {
        }
    }
}
