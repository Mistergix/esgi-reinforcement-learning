using System.Collections.Generic;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.AudioManagement
{
    [CreateAssetMenu(menuName = MenuPaths.MenuBase + "Audio/Audio Track")]
    public class AudioTrack : ScriptableObject
    {
        [SerializeField] private AudioType audioType;
        [SerializeField] private List<AudioData> audios;
        [SerializeField] private List<RandomAudioObject> randomAudioObjects;
        [SerializeField] private bool oneSoundAtOnce = true;
        
        public AudioType AudioType => audioType;
        public List<AudioData> Audios => audios;

        public List<RandomAudioObject> RandomAudioObjects => randomAudioObjects;

        public bool OneSoundAtOnce => oneSoundAtOnce;
    }
}