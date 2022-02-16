using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public interface IAnalyticsProvider
    {
        void Initialize(bool consent);
        void TrackGameStarted(AnalyticsGameStartEventSchema schema);
        void TrackGameFinished(AnalyticsGameOverEventSchema schema);
        void BeginTutorial(AnalyticsTutorialStepEventSchema schema);
        void StoreOpened(AnalyticsShopOpenedEventSchema schema);
        void OpenedUi(AnalyticsUIOpenedEventSchema schema);
    }
}