using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public abstract class AnalyticsEventSchema
    {
        public abstract string EventName { get; }
        public abstract Dictionary<string, object> ToDictionary();
    }
}