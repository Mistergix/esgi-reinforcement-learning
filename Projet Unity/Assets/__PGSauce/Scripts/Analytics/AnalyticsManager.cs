using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public static class AnalyticsManager
    {
        private static readonly List<IAnalyticsProvider> AnalyticsProviders = new List<IAnalyticsProvider>()
        {
            new UnityAnalyticsProvider(), new GameAnalyticsProvider(), new UnityBetaAnalyticsProvider()
        };
        
        public static void Initialize(bool consent)
        {
            AnalyticsProviders.ForEach(provider => provider.Initialize(consent));
        }

        public static void TrackGameStarted(AnalyticsGameStartEventSchema schema)
        {
            AnalyticsProviders.ForEach(provider => provider.TrackGameStarted(schema));
        }
        
        public static void TrackGameFinished(AnalyticsGameOverEventSchema schema)
        {
            AnalyticsProviders.ForEach(provider => provider.TrackGameFinished(schema));
        }
        
        public static void BeginTutorial(AnalyticsTutorialStepEventSchema schema)
        {
            AnalyticsProviders.ForEach(provider => provider.BeginTutorial(schema));
        }

        public static void StoreOpened(AnalyticsShopOpenedEventSchema schema)
        {
            AnalyticsProviders.ForEach(provider => provider.StoreOpened(schema));
        }

        public static void OpenedUI(AnalyticsUIOpenedEventSchema schema)
        {
            AnalyticsProviders.ForEach(provider => provider.OpenedUi(schema));
        }
    }
    
}