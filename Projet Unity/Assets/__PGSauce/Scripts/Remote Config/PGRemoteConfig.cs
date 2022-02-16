using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Services;
using PGSauce.Unity;
using Unity.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace PGSauce.PGRemote
{
    public class PGRemoteConfig : PGMonoBehaviour
    {
        #region Public And Serialized Fields
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        private void OnDestroy()
        {
            var remotes = RemoteConfigTool.Instance.Container.Configs;
            foreach (var remote in remotes)
            {
//                remote.OnReset();
            }
        }

        #endregion
        #region Public Methods
        public void Init()
        {
            var remotes = RemoteConfigTool.Instance.Container.Configs;
            foreach (var remote in remotes)
            {
                remote.FetchRemoteData();
            }
        }
        #endregion
        #region Private Methods
        #endregion

        
    }
}
