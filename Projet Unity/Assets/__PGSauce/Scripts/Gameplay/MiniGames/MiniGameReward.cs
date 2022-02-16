using PGSauce.Unity;
using UnityEngine;

namespace PGSauce.Gameplay.MiniGames
{
    public abstract class MiniGameReward : PGMonoBehaviour
    {
        [SerializeField, Tooltip("How likely is this reward to be chosen")] private float probability = 1;
        public float Probability => probability;
    }
}