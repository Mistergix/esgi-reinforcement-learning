using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class AnalyticsGameOverEventSchema : AnalyticsEventSchema
    {
        private string _levelId;
        private int _score;
        private bool _success;

        public AnalyticsGameOverEventSchema(string levelId, int score, bool success)
        {
            _levelId = levelId;
            _score = score;
            _success = success;
        }

        public string LevelId => _levelId;

        public bool Success => _success;

        public override string EventName => "customGameOver";

        public override Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
                {["levelId"] = LevelId, ["score"] = _score, ["success"] = Success};
        }
    }
}