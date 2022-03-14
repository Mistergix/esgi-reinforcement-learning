using PGSauce.Unity;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class AlgorithmBase : ScriptableObject
    {
        [SerializeField, UnityEngine.Min(1)] private int maxEpochs = 10;

        public int MaxEpochs => maxEpochs;
        public int CurrentEpoch { get; protected set; }
        
        public bool IsRunning { get; protected set; }
        public bool IsTraining { get; protected set; }
    }
}