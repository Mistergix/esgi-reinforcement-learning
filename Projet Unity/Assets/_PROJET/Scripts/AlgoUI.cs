using System;
using PGSauce.Games.IaEsgi.Ia;
using PGSauce.Unity;
using TMPro;
using UnityEngine;

namespace PGSauce.Games.IaEsgi
{
    public class AlgoUI : PGMonoBehaviour
    {
        [SerializeField] private MonoRlBase game;
        [SerializeField] private TMP_Text epochText;
        [SerializeField] private TMP_Text stateText;

        private void Update()
        {
            epochText.text = $"Epoch : {game.AlgorithmBase.CurrentEpoch} / {game.AlgorithmBase.MaxEpochs}";
            var state = "";
            if (game.AlgorithmBase.IsRunning)
            {
                state = "RUNNING";
            }

            if (game.AlgorithmBase.IsTraining)
            {
                state = "TRAINING";
            }

            stateText.text = $"{state}";
        }
    }
}