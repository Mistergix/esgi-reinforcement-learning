using System.Collections.Generic;
using System.IO;
using PGSauce.Core;
using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PGSauce.AudioManagement
{
    [CreateAssetMenu(menuName = MenuPaths.MenuBase + "Audio/Audio Object (Random)")]
    public class RandomAudioObject : ScriptableObject
    {
        [SerializeField] private List<AudioObject> audioObjects;
        [SerializeField, InfoBox("-1 for always random")] private int forceIndex = -1;
        [SerializeField] private bool randomOnce;
        
        [SerializeField, Range(0, 1), FoldoutGroup("Generate Audio Objects")] private float initialVolume = 1;
        [SerializeField, FoldoutGroup("Generate Audio Objects")] private bool loop;
        [SerializeField, Min(0), FoldoutGroup("Generate Audio Objects")] private float fadeInSeconds;
        [SerializeField, Min(0), FoldoutGroup("Generate Audio Objects")] private float fadeOutSeconds;
        [ShowInInspector, FoldoutGroup("Generate Audio Objects")]
        private List<AudioClip> clips;
        
        private int _selectedIndex;

        public AudioObject RandomAudio => audioObjects[GetIndex()];

        private int GetIndex()
        {
#if UNITY_EDITOR
            if (forceIndex >= 0)
            {
                return forceIndex;
            }
#endif
            return randomOnce ? _selectedIndex : audioObjects.GetRandomIndex();
        }

        public bool Loop => loop;

        public float InitialVolume => initialVolume;

        public float FadeInSeconds => fadeInSeconds;

        public float FadeOutSeconds => fadeOutSeconds;

        public List<AudioObject> AudioObjects => audioObjects;

        public void Init()
        {
            _selectedIndex = audioObjects.GetRandomIndex();
        }
    }
}