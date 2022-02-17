using System;
using PGSauce.Games.IaEsgi.Ia;
using PGSauce.Unity;
using TMPro;
using UnityEngine;

namespace PGSauce.Games.IaEsgi
{
    public class AlgoUI : PGMonoBehaviour
    {
        [SerializeField] private AlgorithmBase algo;
        [SerializeField] private TMP_Text epochText;

        private void Update()
        {
            epochText.text = $"Epoch : {algo.CurrentEpoch} / {algo.MaxEpochs}";
        }
    }
}