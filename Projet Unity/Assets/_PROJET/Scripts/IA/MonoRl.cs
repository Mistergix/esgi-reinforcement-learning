using System;
using PGSauce.Unity;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class MonoRl<TGame, TAgent, TState> : MonoRlBase where TAgent : QAgentBase where TState : QState where TGame : MonoRlBase
    {
        [SerializeField] private RlAlgorithm<TGame, TAgent, TState> algorithm;

        protected RlAlgorithm<TGame, TAgent, TState> Algorithm => algorithm;

        public override AlgorithmBase AlgorithmBase => Algorithm;

        protected QAgent<TAgent, TState> Agent => Algorithm.Agent;

        private void Awake()
        {
            Algorithm.Init(this as TGame);
            GameCustomInit();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TryTrainAgent();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                TryRunTrainedAgent();
            }

            GameCustomUpdate();
        }

        protected abstract void GameCustomUpdate();

        protected abstract void GameCustomInit();

        private void TryTrainAgent()
        {
            if (Algorithm.IsBusy)
            {
                return;
            }

            StartCoroutine(Algorithm.Train());
        }

        private void TryRunTrainedAgent()
        {
            if (Algorithm.IsBusy)
            {
                return;
            }
            
            StartCoroutine(Algorithm.RunTrainedAgent());
        }
    }
}