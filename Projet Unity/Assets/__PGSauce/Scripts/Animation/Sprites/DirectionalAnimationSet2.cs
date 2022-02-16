using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using PGSauce.Core.Strings;

namespace PGSauce.Animation
{
    [CreateAssetMenu(menuName = MenuPaths.MenuBase + "Animations/Directional Animation 1D")]
    public class DirectionalAnimationSet2 : ScriptableObject
    {
        [SerializeField] private AnimationClip left;
        [SerializeField] private AnimationClip right;
        [SerializeField] private float speed = 1;

        public AnimationClip Right => right;

        public AnimationClip Left => left;

        public float Speed => speed;

        public AnimationClip GetClip(Vector2 direction)
        {
            return direction.x > 0 ? right : left;
        }
    }
}
