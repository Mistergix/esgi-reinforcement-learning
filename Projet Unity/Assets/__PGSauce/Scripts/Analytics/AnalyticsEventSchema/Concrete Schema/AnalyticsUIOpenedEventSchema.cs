using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class AnalyticsUIOpenedEventSchema : AnalyticsEventSchema
    {
        private string _uiName;

        public AnalyticsUIOpenedEventSchema(string uiName)
        {
            _uiName = uiName;
        }

        public string UIName => _uiName;

        public override string EventName => "openedUi";

        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() {["uiName"] = UIName};
        }
    }
}