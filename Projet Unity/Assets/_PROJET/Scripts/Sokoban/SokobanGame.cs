using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class SokobanGame : MonoRl<SokobanGame, QAgentSokoban, QStateSokoban>
    {
        public override void ResetGameForNewEpoch()
        {
            throw new System.NotImplementedException();
        }

        protected override void GameCustomInit()
        {
            throw new System.NotImplementedException();
        }

        protected override void GameCustomUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
