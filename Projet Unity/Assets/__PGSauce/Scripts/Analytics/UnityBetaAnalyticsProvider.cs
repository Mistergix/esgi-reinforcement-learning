using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using PGSauce.Analytics;
using PGSauce.Services;
using Unity.Services.Analytics;
using Unity.Services.Core;

namespace PGSauce.Analytics
{
    public class UnityBetaAnalyticsProvider : IAnalyticsProvider
    {
        public async void Initialize(bool consent)
        {
        }

        public void TrackGameStarted(AnalyticsGameStartEventSchema schema)
        {
            Events.CustomData(schema.EventName, schema.ToDictionary());
        }

        public void TrackGameFinished(AnalyticsGameOverEventSchema schema)
        {
            Events.CustomData(schema.EventName, schema.ToDictionary());
        }

        public void BeginTutorial(AnalyticsTutorialStepEventSchema schema)
        {
            Events.CustomData(schema.EventName, schema.ToDictionary());
        }

        public void StoreOpened(AnalyticsShopOpenedEventSchema schema)
        {
            Events.CustomData(schema.EventName, schema.ToDictionary());
        }

        public void OpenedUi(AnalyticsUIOpenedEventSchema schema)
        {
            Events.CustomData(schema.EventName, schema.ToDictionary());
        }
    }
}
