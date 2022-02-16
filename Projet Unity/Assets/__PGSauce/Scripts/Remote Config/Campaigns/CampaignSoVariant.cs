using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Strings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PGSauce.PGRemote
{
    [CreateAssetMenu(menuName = MenuPaths.RemoteCampaigns + "Variant (AB TEST)")]
    public class CampaignSoVariant : CampaignSoBase<CampaignTypeVariant>
    {
        [SerializeField, ValidateInput("VariantsAreValid", WeightSumErrorMessage + ", and there must be at least on variant")] private List<Variant> variants = new List<Variant>();
        private const string WeightSumErrorMessage = "Variants Weights sum must be 100";

        public List<Variant> Variants => variants;

        protected override void ModifyCampaign(CampaignTypeVariant campaign)
        {
            if (!VariantsAreValid())
            {
                PGDebug.Message(WeightSumErrorMessage).Log();
                throw new Exception(WeightSumErrorMessage);
            }
            campaign.value = variants;
        }

        /// <summary>
        /// CALLED ODIN
        /// </summary>
        /// <returns></returns>
        private bool VariantsAreValid()
        {
            return AtLeastOneVariant() && VariantsSumIs100();
        }

        private bool AtLeastOneVariant()
        {
            return variants != null && variants.Count > 0;
        }

        private bool VariantsSumIs100()
        {
            return variants.Sum(variant => variant.weight) == 100;
        }

        [Serializable]
        public struct Variant
        {
            public string id;
            [Range(0, 100), InfoBox("The weight sum for all the variants must equal 100")]
            public int weight;

            public string type => "variant";
            [ValidateInput("ValidateVariants", "One key can only be used once in each variant!")]
            public List<RemoteConfigTool.Value> values;

            private bool ValidateVariants()
            {
                var keys = values.Select(value => value.key).ToList();
                if(keys.Count != keys.Distinct().Count())
                {
                    return false;
                }

                return true;
            }
        }

        public Variant GetVariantById(string id)
        {
            return variants.FirstOrDefault(v => v.id.Equals(id));
        }
    }
}