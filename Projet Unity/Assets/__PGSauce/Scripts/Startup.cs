using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using PGSauce.Analytics;
using PGSauce.Core.PGDebugging;
using PGSauce.PGRemote;
using PGSauce.Save;
using PGSauce.Services;
using PGSauce.Unity;
using UnityEngine.SceneManagement;

namespace PGSauce.PGStartup
{
    public class Startup : PGMonoBehaviour
    {
        [SerializeField] private PGAnalytics analytics;
        [SerializeField] private PGRemoteConfig remote;
        
#if UNITY_ASSERTIONS
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void DisableAnimancerWarnings()
        {
            Animancer.OptionalWarning.EndEventInterrupt.Disable();
    
            // You could disable OptionalWarning.All, but that is not recommended for obvious reasons.
        }
#endif

        async void Awake()
        {
            SceneManager.LoadScene(1);
            return;
            
            if (PgSave.Instance.IsGameLaunchedForTheFirstTime)
            {
                PGDebug.Message("Game Launched For the first time").Log();
                PgSave.Instance.GameLaunchedForTheFirstTime.SaveData(false);
            }
            else
            {
                PGDebug.Message("Game NOT Launched the first time").Log();
            }

            await PGServices.Instance.InitServices();
            analytics.Init();
            remote.Init();
            SceneManager.LoadScene(1);
        }
    }
}
