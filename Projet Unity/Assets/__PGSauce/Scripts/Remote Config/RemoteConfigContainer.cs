using System.Collections.Generic;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.PGRemote
{
    [CreateAssetMenu (menuName = MenuPaths.Remote + "Container")]
    public class RemoteConfigContainer : ScriptableObject
    {
        [SerializeField] private List<RemoteConfigValuesProvider> configs;
        [SerializeField] private List<CampaignSo> campaigns;

        public List<RemoteConfigValuesProvider> Configs => configs;

        public List<CampaignSo> Campaigns => campaigns;
    }
}