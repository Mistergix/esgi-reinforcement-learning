using System;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using PGSauce.Core.PGDebugging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.PGRemote
{
    public abstract class CampaignSoBase<TCampaign> : CampaignSo where TCampaign : CampaignTypeBase, new()
    {
        [SerializeField] private bool campaignEnabled;
        [SerializeField, TextArea, InfoBox("The Condition is a JEXL expression of contextual data attributes used to define the audience for which you want a campaign to apply")] private string condition = "true"; 
        [SerializeField, Range(0, 100), InfoBox("The percentage of users meeting the condition that will adhere to this campaign")] private int rolloutPercentage = 100; 
        [SerializeField, Range(1, 1000), InfoBox("Between 1 (highest priority) and 1000 (lowest priority)")] private int priority = 1000;
        [SerializeField] private bool hasStartDate;
        [SerializeField] private bool hasEndDate;
        [SerializeField, ShowIf("@hasStartDate")] private Date startDate = DefaultDate();
        [SerializeField, ShowIf("@hasEndDate")] private Date endDate = DefaultDate();

        [ShowInInspector]
        public string StartDate => hasStartDate ? startDate.ToString() : null;
        [ShowInInspector]
        public string EndDate => hasEndDate ? endDate.ToString() : null;
        
        [ShowInInspector]
        public string ReadableStartDate => hasStartDate ? startDate.Readable() : null;
        [ShowInInspector]
        public string ReadableEndDate => hasEndDate ? endDate.Readable() : null;

        public bool CampaignEnabled => campaignEnabled;

        public int Priority => priority;

        public sealed override StringContent GetData()
        {
            var campaign = GetCampaign();
            ModifyCampaign(campaign);
            var json = JsonConvert.SerializeObject(campaign);
            PGDebug.Message($"Campaign JSON is {json}").Log();
            return new StringContent(json, System.Text.Encoding.UTF8,
                "application/json");
        }

        protected abstract void ModifyCampaign(TCampaign campaign);

        protected TCampaign GetCampaign()
        {
            var campaign = new TCampaign();
            campaign.environmentId = RemoteConfigTool.Instance.DefaultEnvironment.id;
            campaign.configId = RemoteConfigTool.Instance.DefaultConfigs.id;
            campaign.name = CampaignName;
            campaign.condition = condition;
            campaign.enabled = campaignEnabled;
            campaign.rolloutPercentage = rolloutPercentage;
            campaign.priority = priority;
            campaign.startDate = StartDate;
            campaign.endDate = EndDate;
            return campaign;
        }
        
        private static Date DefaultDate()
        {
            return new Date(){year = 2021, hour = 0, month = 1, day = 1};
        }
        
        [Serializable]
        private struct Date
        {
            [Range(2000, 9999)]
            public int year;
            [Range(1, 12)]
            public int month;
            [Range(1, 31)]
            public int day;
            [Range(0, 23)] public int hour;

            public override string ToString()
            {
                return $"{new DateTime(year, month, day, hour, 0, 0):yyyy-MM-ddTHH:mm:ssZ}";
            }

            public string Readable()
            {
                return new DateTime(year, month, day, hour, 0, 0).ToString("f", CultureInfo.InvariantCulture);
            }
        }
    }
}