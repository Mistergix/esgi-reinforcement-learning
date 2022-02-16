using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;

namespace PGSauce.Animation
{
    public class CameraShaker : MonoSingleton<CameraShaker>
    {
        [SerializeField] private CinemachineVirtualCamera cam;
        public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
        public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
        public float ShakeFrequency = 2.0f;

        private CinemachineBasicMultiChannelPerlin noise;

        void Start()
        {
            noise = cam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            EndShake();
        }

        private bool shaking;

        public void ShakeShortly()
        {
            if (shaking) { return; }
            BeginShake();
            DOVirtual.DelayedCall(ShakeDuration, () => EndShake());
            PGDebug.Message("Do Haptic").LogTodo();
            }

        public void EndShake()
        {
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
            shaking = false;
        }

        public void BeginShake()
        {
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;
            shaking = true;
        }
    }
}
