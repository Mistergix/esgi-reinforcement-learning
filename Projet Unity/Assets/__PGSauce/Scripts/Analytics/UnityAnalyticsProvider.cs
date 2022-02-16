using System.Collections.Generic;
using UnityEngine.Analytics;

namespace PGSauce.Analytics
{
    public class UnityAnalyticsProvider : IAnalyticsProvider
    {
        public void Initialize(bool consent)
        {
            
        }

        public void TrackGameStarted(AnalyticsGameStartEventSchema schema)
        {
            AnalyticsEvent.GameStart(schema.ToDictionary());
        }

        public void TrackGameFinished(AnalyticsGameOverEventSchema schema)
        {
            AnalyticsEvent.GameOver(schema.LevelId, schema.ToDictionary());
        }

        public void BeginTutorial(AnalyticsTutorialStepEventSchema schema)
        {
            AnalyticsEvent.TutorialStart(schema.TutoId, schema.ToDictionary());
        }

        public void StoreOpened(AnalyticsShopOpenedEventSchema schema)
        {
            AnalyticsEvent.StoreOpened(StoreType.Soft, schema.ToDictionary());
        }

        public void OpenedUi(AnalyticsUIOpenedEventSchema schema)
        {
            AnalyticsEvent.ScreenVisit(schema.UIName, schema.ToDictionary());
        }
    }
}