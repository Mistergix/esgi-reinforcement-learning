using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class AnalyticsGameStartEventSchema : AnalyticsEventSchema
    {
        private string _levelId;

        public AnalyticsGameStartEventSchema(string levelId)
        {
            _levelId = levelId;
        }

        public string LevelId => _levelId;

        public override string EventName => "customGameStarted";

        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() {["levelId"] = LevelId};
        }
    }
}