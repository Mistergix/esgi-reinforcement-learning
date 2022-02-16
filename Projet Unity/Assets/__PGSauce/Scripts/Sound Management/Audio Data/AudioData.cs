using UnityEngine;

namespace PGSauce.AudioManagement
{
    public abstract class AudioData : ScriptableObject
    {
        [SerializeField, Range(0, 1)] private float initialVolume = 1;
        [SerializeField] private bool loop;
        [SerializeField, Min(0)] private float fadeInSeconds;
        [SerializeField, Min(0)] private float fadeOutSeconds;

        public abstract AudioClip Clip
        {
            get;
        }
        
        public float InitialVolume
        {
            get => initialVolume;
            protected set => initialVolume = value;
        }

        public bool Loop
        {
            get => loop;
            protected set => loop = value;
        }

        public float FadeInSeconds
        {
            get => fadeInSeconds;
            protected set => fadeInSeconds = value;
        }

        public float FadeOutSeconds
        {
            get => fadeOutSeconds;
            protected set => fadeOutSeconds = value;
        }
    }
}