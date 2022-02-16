using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.PGRemote.ABTest;
using Unity.RemoteConfig;
using UnityEngine;

namespace PGSauce.PGRemote
{
    public abstract class RemoteConfigValuesProvider : ScriptableObject
    {
        private static ABTestHandler ABTest => ABTestHandler.Instance;
        
        public struct userAttributes {
        }

        public struct appAttributes {
        }

        public IEnumerable<RemoteConfigTool.Value> Values => GetValues().AsReadOnly();

        protected abstract List<RemoteConfigTool.Value> GetValues();

        public void FetchRemoteData()
        {
            ConfigManager.FetchCompleted += OnFetchCompleted;
            ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
        }
        
        protected abstract void OnCustomFetchCompleted();

        protected abstract void LoadDefaultValues();

        public void OnReset()
        {
            ConfigManager.FetchCompleted -= OnFetchCompleted;
        }
        
        public float GetFloat(string key)
        {
            return ABTest.GetFloat(key);
        }

        public int GetInt(string key)
        {
            return ABTest.GetInt(key);
        }
        
        public long GetLong(string key)
        {
            return ABTest.GetLong(key);
        }
        
        public bool GetBool(string key)
        {
            return ABTest.GetBool(key);
        }
        
        public string GetString(string key)
        {
            return ABTest.GetString(key);
        }

        private void OnFetchCompleted(ConfigResponse response)
        {
            switch (response.requestOrigin) {
                case ConfigOrigin.Default:
                    PGDebug.Message("Get Default Values").Log();
                    LoadDefaultValues();
                    break;
                case ConfigOrigin.Cached:
                    PGDebug.Message("Get Values from cache").Log();
                    OnCustomFetchCompleted();
                    break;
                case ConfigOrigin.Remote:
                    PGDebug.Message("New settings loaded this session; update values accordingly.").Log();
                    OnCustomFetchCompleted();
                    break;
            }
        }
    }
}