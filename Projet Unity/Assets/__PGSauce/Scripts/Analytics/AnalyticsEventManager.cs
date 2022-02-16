using UnityEngine;
using System.Collections.Generic;
using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;

namespace PGSauce.Analytics
{
    [CreateAssetMenu(menuName = MenuPaths.Settings +  "Analytics Event Manager")]
    public class AnalyticsEventManager : SOSingleton<AnalyticsEventManager>
    {
        public void OpenedUI(AnalyticsUIOpenedEventSchema schema)
        {
            AnalyticsManager.OpenedUI(schema);
        }

        public void StoreOpened(AnalyticsShopOpenedEventSchema schema)
        {
            AnalyticsManager.StoreOpened(schema);
        }

        public void NewGameStarted(AnalyticsGameStartEventSchema schema)
        {
            AnalyticsManager.TrackGameStarted(schema);
        }

        public void OnGameOver(AnalyticsGameOverEventSchema schema)
        { 
            AnalyticsManager.TrackGameFinished(schema);
        }

        public void BeginTutorial(AnalyticsTutorialStepEventSchema tutorialStepEventSchema)
        {
            AnalyticsManager.BeginTutorial(tutorialStepEventSchema);
        }
    }
}
