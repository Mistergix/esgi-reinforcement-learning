namespace PGSauce.PGRemote
{
    public abstract class CampaignTypeBase
    {
        public string environmentId;
        public string configId;
        public string name;
        public abstract string type { get; }
        public string condition;
        public bool enabled;
        public string startDate;
        public string endDate;
        public int rolloutPercentage;
        public int priority;
    }
}