using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using PGSauce.Core.PGDebugging;
using PGSauce.Unity;
using UnityEngine.SceneManagement;

namespace PGSauce.PGStartup
{
    public class Startup : PGMonoBehaviour
    {
        
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
        }
    }
}
