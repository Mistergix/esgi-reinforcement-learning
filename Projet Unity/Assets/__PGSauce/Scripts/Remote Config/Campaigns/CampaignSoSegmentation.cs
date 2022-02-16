using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Strings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.PGRemote
{
    [CreateAssetMenu(menuName = MenuPaths.RemoteCampaigns + "Segmentation")]
    public class CampaignSoSegmentation : CampaignSoBase<CampaignTypeSegmentation>
    {
        [SerializeField, ValidateInput("AtLeastOneSetting", SettingsMessage)] private List<Values> settings = new List<Values>();
        
        private const string SettingsMessage = "At least one setting !";
        protected override void ModifyCampaign(CampaignTypeSegmentation campaign)
        {
            if (!AtLeastOneSetting())
            {
                PGDebug.Message(SettingsMessage).Log();
                throw new Exception(SettingsMessage);
            }
            campaign.value = settings;
        }
        
        private bool AtLeastOneSetting()
        {
            return settings != null && settings.Count > 0;
        }
        
        [Serializable]
        public struct Values
        {
            [ValueDropdown("@RemoteConfigTool.SupportedKeys")]
            public string key;
            [ValueDropdown("@RemoteConfigTool.SupportedTypes")]
            public string type;
            [InfoBox("Only the first one is used, Unity API duh")]
            public List<string> values;
        }
    }
}