using System;
using DG.Tweening;
using UnityEngine;

namespace PGSauce.Animation
{
    [Serializable]
    public struct DotweenProfile
    {
        [SerializeField] private Ease ease;
        [SerializeField, Min(0)] private float duration;

        public float Duration => duration;

        public Ease Ease => ease;
    }
}