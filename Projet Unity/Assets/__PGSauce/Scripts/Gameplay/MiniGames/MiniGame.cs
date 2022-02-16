using System;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace PGSauce.Gameplay.MiniGames
{
    public abstract class MiniGame<T> : PGMonoBehaviour where T : MiniGameReward
    {
        [SerializeField] private Transform rewardsParents;
        [SerializeField] private UnityEvent onStart;

        protected List<T> Rewards { get; private set; }

        protected void Awake()
        {
            SetupGame();
        }

        /// <summary>
        /// Init the game
        /// </summary>
        private void SetupGame()
        {
            var rewards = rewardsParents.GetComponentsInChildren<T>(true);
            Rewards = rewards.ToList();
            FilterRewards();
            foreach (var gameReward in rewards)
            {
               //gameReward.Hide();
            }

            Rewards.RemoveAll(r => r.Probability <= 0);
            
            ApplyLayout();
            onStart.Invoke();
        }

        /// <summary>
        /// Filter the Rewards list, it removes the rewards that shouldn't be used according to the gameplay (some powerups, need to be unlocked, some gifts are not available yet, etc)
        /// </summary>
        protected virtual void FilterRewards()
        {
            
        }

        /// <summary>
        /// Arrange the game elements
        /// </summary>
        protected abstract void ApplyLayout();
    }
}