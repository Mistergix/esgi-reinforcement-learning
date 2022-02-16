using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class AnalyticsShopOpenedEventSchema : AnalyticsEventSchema
    {
        private string _shopType;

        public AnalyticsShopOpenedEventSchema(string shopType)
        {
            _shopType = shopType;
        }

        public override string EventName => "storeOpened";

        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() {["shopType"] = _shopType};
        }
    }
}