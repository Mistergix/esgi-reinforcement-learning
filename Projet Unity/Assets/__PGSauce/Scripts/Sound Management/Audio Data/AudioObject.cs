using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.AudioManagement
{
    [CreateAssetMenu(menuName = MenuPaths.MenuBase + "Audio/Audio Object")]
    public class AudioObject : AudioData
    {
        [SerializeField] private AudioClip clip;
        public override AudioClip Clip => clip;

        public void CreateFromRandomAudioObject(RandomAudioObject randomAudioObject, AudioClip audioClip)
        {
            InitialVolume = randomAudioObject.InitialVolume;
            Loop = randomAudioObject.Loop;
            FadeInSeconds = randomAudioObject.FadeInSeconds;
            FadeOutSeconds = randomAudioObject.FadeOutSeconds;
            clip = audioClip;
        }
    }
}