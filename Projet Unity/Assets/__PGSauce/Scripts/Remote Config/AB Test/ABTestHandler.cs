using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using Unity.RemoteConfig;

namespace PGSauce.PGRemote.ABTest
{
    [CreateAssetMenu(menuName = MenuPaths.ABTest + "Debug Singleton")]
    public class ABTestHandler : SOSingleton<ABTestHandler>
    {
        [SerializeField] private bool debug;

        [CalledByOdin] public static List<string> SupportedKeys => Instance.GetSettingsKeys();

        private RemoteConfigContainer Container => RemoteConfigTool.Instance.Container;

        [ShowInInspector]
        private List<CampaignSoVariant> AbTests =>
            Container.Campaigns.Where(campaign => campaign is CampaignSoVariant).Cast<CampaignSoVariant>().ToList();

        [ShowInInspector]
        private List<CampaignSoVariant> EnabledAbTests => AbTests.Where(abtest => abtest.CampaignEnabled).ToList();
        
        [ShowInInspector]
        private List<CampaignSoVariant> DisabledAbTests => AbTests.Where(abtest => !abtest.CampaignEnabled).ToList();

        public List<DebugAbTest> DebugAbTests => debugAbTests;

        public bool Debug
        {
            get => debug;
            set => debug = value;
        }

        [SerializeField] private List<DebugAbTest> debugAbTests;

        public float GetFloat(string key)
        {
            if (Debug)
            {
                bool hasDebugAbTest  = TryGetDebugValue(key, out var stringValue);

                if (hasDebugAbTest)
                {
                    return float.Parse(stringValue);
                }
            }
            
            return ConfigManager.appConfig.GetFloat(key);
        }

        public int GetInt(string key)
        {
            if (Debug)
            {
                bool hasDebugAbTest  = TryGetDebugValue(key, out var stringValue);

                if (hasDebugAbTest)
                {
                    return int.Parse(stringValue);
                }
            }
            
            return ConfigManager.appConfig.GetInt(key);
        }
        
        public long GetLong(string key)
        {
            if (Debug)
            {
                bool hasDebugAbTest  = TryGetDebugValue(key, out var stringValue);

                if (hasDebugAbTest)
                {
                    return long.Parse(stringValue);
                }
            }
            
            return ConfigManager.appConfig.GetLong(key);
        }
        
        public bool GetBool(string key)
        {
            if (Debug)
            {
                bool hasDebugAbTest  = TryGetDebugValue(key, out var stringValue);

                if (hasDebugAbTest)
                {
                    return bool.Parse(stringValue);
                }
            }
            
            return ConfigManager.appConfig.GetBool(key);
        }
        
        public string GetString(string key)
        {
            if (Debug)
            {
                bool hasDebugAbTest  = TryGetDebugValue(key, out var stringValue);

                if (hasDebugAbTest)
                {
                    return stringValue;
                }
            }
            
            return ConfigManager.appConfig.GetString(key);
        }

        [Button]
        private void RegenerateDebugList()
        {
            debugAbTests = new List<DebugAbTest>();
            var keys = RemoteConfigTool.SupportedKeys;

            foreach (var key in keys)
            {
                var abTest = new DebugAbTest(key);
                if (abTest.abTest != null)
                {
                    DebugAbTests.Add(abTest);
                }
            }
        }
        
        private bool TryGetDebugValue(string key, out string stringValue)
        {
            var hasDebugAbTest = DebugAbTests.Any(ab => ab.key.Equals(key));
            stringValue = "";
            if (hasDebugAbTest)
            {
                var abTestDebug = DebugAbTests.FirstOrDefault(ab => ab.key.Equals(key));
                if (abTestDebug.abTest == null)
                {
                    return false;
                }
                var variant = abTestDebug.abTest.GetVariantById(abTestDebug.variant);
                if (!variant.id.IsNullOrEmpty())
                {
                    var value = variant.values.FirstOrDefault(v => v.key.Equals(key));
                    stringValue = value.value;
                }
            }

            return hasDebugAbTest;
        }
        
        private List<string> GetSettingsKeys()
        {
            var abtests = AbTests;
            var keys = new HashSet<string>();
            foreach (var abtest in abtests)
            {
                foreach (var variant in abtest.Variants)
                {
                    foreach (var value in variant.values)
                    {
                        keys.Add(value.key);
                    }
                }
            }

            return keys.ToList();
        }
        
        private List<CampaignSoVariant> GetAbTestsThatHaveThisKey(string key)
        {
            var abtests = AbTests;
            var result = new List<CampaignSoVariant>();

            foreach (var abtest in abtests)
            {
                if (abtest.Variants.Any(variant => variant.values.Any(value => value.key.Equals(key))))
                {
                    result.Add(abtest);
                }
            }

            return result;
        }
        

        [Serializable]
        public struct DebugAbTest
        {
            [ValueDropdown("@ABTestHandler.SupportedKeys"), ReadOnly]
            public string key;
            [ValueDropdown("SupportedAbTests"), OnValueChanged("OnAbTestChanged")]
            public CampaignSoVariant abTest;
            [ValueDropdown("SupportedVariants")]
            public string variant;

            [ShowInInspector] private string Value => GetValue();

            public DebugAbTest(string key)
            {
                this.key = key;
                abTest = null;
                variant = "";
                var tests = SupportedAbTests();
                if (tests.Count > 0)
                {
                    abTest = tests[0];
                }
                UpdateVariant();
            }

            private void UpdateVariant()
            {
                var variants = SupportedVariants();
                if (variants.Count > 0)
                {
                    variant = variants[0];
                }
            }

            public DebugAbTest(string key, CampaignSoVariant abtest)
            {
                this.key = key;
                this.abTest = abtest;
                variant = "";
                UpdateVariant();
            }
            
            public DebugAbTest(string key, CampaignSoVariant abtest, string variant)
            {
                this.key = key;
                this.abTest = abtest;
                this.variant = variant;
            }

            [CalledByOdin]
            private void OnAbTestChanged()
            {
                UpdateVariant();
            }

            public List<CampaignSoVariant> SupportedAbTests()
            {
                if (key.IsNullOrEmpty())
                {
                    return new List<CampaignSoVariant>();
                }

                return Instance.GetAbTestsThatHaveThisKey(key).ToList();
            }
            
            public List<string> SupportedVariants()
            {
                if (abTest == null)
                {
                    return new List<string>();
                }

                var thisKey = key;
                return abTest.Variants.Where(v => v.values.Any(value => value.key.Equals(thisKey))).Select(v => v.id).ToList();
            }

            public string GetValue()
            {
                var hasDebugAbTest  = Instance.TryGetDebugValue(key, out var stringValue);
                return hasDebugAbTest ? stringValue : "";
            }
        }
    }
}
