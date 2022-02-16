using System.Collections.Generic;

namespace PGSauce.PGRemote
{
    public class CampaignTypeSegmentation : CampaignTypeBase
    {
        public override string type => "segmentation";
        public List<CampaignSoSegmentation.Values> value;
    }
}