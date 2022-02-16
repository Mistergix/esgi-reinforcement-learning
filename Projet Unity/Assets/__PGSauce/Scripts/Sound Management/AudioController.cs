using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hellmade.Sound;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;

namespace PGSauce.AudioManagement
{
    public class AudioController : MonoSingleton<AudioController>
    {
        [SerializeField] private List<AudioTrack> audioTracks;
        [SerializeField] private bool debug;

        private Dictionary<AudioData, Audio> _audioTable;
        private Dictionary<AudioData, AudioTrack> _audioTracksTable;
        
        public float GlobalVolume
        {
            get => EazySoundManager.GlobalVolume;
            set => EazySoundManager.GlobalVolume = value;
        }
        
        public float MusicVolume
        {
            get => EazySoundManager.GlobalMusicVolume;
            set => EazySoundManager.GlobalMusicVolume = value;
        }
        
        public float SFXVolume
        {
            get => EazySoundManager.GlobalSoundsVolume;
            set => EazySoundManager.GlobalSoundsVolume = value;
        }
        
        public float UIVolume
        {
            get => EazySoundManager.GlobalUISoundsVolume;
            set => EazySoundManager.GlobalUISoundsVolume = value;
        }
        
        
        
        public void Play(AudioData audioObject, float pitch = 1)
        {
            if (!_audioTable.ContainsKey(audioObject))
            {
                PGDebug.Message($"{audioObject} is not a key of {name} ! No sound Played").LogWarning();
                return;
            }

            var track = _audioTracksTable[audioObject];
            if (track.OneSoundAtOnce)
            {
                var stopAudios = track.Audios.Select(audioObj => _audioTable[audioObj])
                    .Where(au => au.IsPlaying && au != _audioTable[audioObject]);
                foreach (var au in stopAudios)
                {
                    au.Stop();
                }
            }

            _audioTable[audioObject].Pitch = pitch;
            _audioTable[audioObject].Play(audioObject.InitialVolume);
            PGDebug.SetCondition(debug).Message($"[AUDIO] Play : {audioObject}").Log();
        }

        public override void Init()
        {
            base.Init();
            _audioTable = new Dictionary<AudioData, Audio>();
            _audioTracksTable = new Dictionary<AudioData, AudioTrack>();

            foreach (var audioTrack in audioTracks)
            {
                foreach (var audioObject in audioTrack.Audios)
                {
                    if (!GetAudio(audioObject, audioTrack, out var audio)) continue;

                    _audioTable.Add(audioObject, audio);
                    _audioTracksTable.Add(audioObject, audioTrack);
                }
                foreach (var random in audioTrack.RandomAudioObjects)
                {
                    random.Init();
                    foreach (var audioObject in random.AudioObjects)
                    {
                        if (!GetAudio(audioObject, audioTrack, out var audio)) continue;

                        _audioTable.Add(audioObject, audio);
                        _audioTracksTable.Add(audioObject, audioTrack);
                    }
                }
            }
        }

        private bool GetAudio(AudioData audioObject, AudioTrack audioTrack, out Audio audio)
        {
            audio = null;
            if (_audioTable.ContainsKey(audioObject))
            {
                PGDebug.Message($"{audioObject} already registered in {name}, skipped").LogWarning();
                return false;
            }
            
            switch (audioTrack.AudioType)
            {
                case AudioType.Music:
                    var id = EazySoundManager.PrepareMusic(audioObject.Clip, audioObject.InitialVolume,
                        audioObject.Loop, true, audioObject.FadeInSeconds, audioObject.FadeOutSeconds);
                    audio = EazySoundManager.GetMusicAudio(id);
                    break;
                case AudioType.Sfx:
                    id = EazySoundManager.PrepareSound(audioObject.Clip, audioObject.InitialVolume,
                        audioObject.Loop, null);
                    audio = EazySoundManager.GetSoundAudio(id);
                    break;
                case AudioType.Ui:
                    id = EazySoundManager.PrepareUISound(audioObject.Clip, audioObject.InitialVolume);
                    audio = EazySoundManager.GetUISoundAudio(id);
                    break;
            }

            if (audio == null)
            {
                PGDebug.Message($"Id not set for {audioObject}").LogWarning();
                return false;
            }

            audio.Persist = true;

            return true;
        }
    }
}
