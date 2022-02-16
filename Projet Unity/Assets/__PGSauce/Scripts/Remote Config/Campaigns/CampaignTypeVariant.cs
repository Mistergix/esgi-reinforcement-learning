using System.Collections.Generic;

namespace PGSauce.PGRemote
{
    public class CampaignTypeVariant : CampaignTypeBase
    {
        public override string type => "variant";
        public List<CampaignSoVariant.Variant> value;
    }
}