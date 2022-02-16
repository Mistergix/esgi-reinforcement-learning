using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class AnalyticsTutorialStepEventSchema : AnalyticsEventSchema
    {
        private string _tutoId;
        private int _tutoStep;

        public AnalyticsTutorialStepEventSchema(string tutoId, int tutoStep)
        {
            _tutoId = tutoId;
            _tutoStep = tutoStep;
        }

        public string TutoId => _tutoId;

        public override string EventName => "tutoStep";

        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() {["tutoId"] = TutoId, ["tutoStep"] = _tutoStep};
        }
    }
}