using System.Net.Http;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.PGRemote
{
    public abstract class CampaignSo : ScriptableObject
    {
        [SerializeField, Required] private string campaignName;
        [SerializeField, ReadOnly] private string campaignId;
        public abstract StringContent GetData();
        public string CampaignName => campaignName;
        public string CampaignId { get => campaignId; set => campaignId = value; }
    }
}