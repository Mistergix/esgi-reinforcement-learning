using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

namespace PGSauce.Analytics
{
    public class GameAnalyticsProvider : IAnalyticsProvider, IGameAnalyticsATTListener
    {
        public void Initialize(bool consent)
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                InitializeGA();
            }
        }

        public void TrackGameStarted(AnalyticsGameStartEventSchema schema)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, schema.LevelId, schema.ToDictionary());
        }

        public void TrackGameFinished(AnalyticsGameOverEventSchema schema)
        {
            var status = schema.Success ? GAProgressionStatus.Complete : GAProgressionStatus.Fail;
            GameAnalytics.NewProgressionEvent(status, schema.LevelId, schema.ToDictionary());
        }

        public void BeginTutorial(AnalyticsTutorialStepEventSchema schema)
        {
            GameAnalytics.NewDesignEvent(schema.EventName, schema.ToDictionary());
        }

        public void StoreOpened(AnalyticsShopOpenedEventSchema schema)
        {
            GameAnalytics.NewDesignEvent(schema.EventName, schema.ToDictionary());
        }

        public void OpenedUi(AnalyticsUIOpenedEventSchema schema)
        {
            GameAnalytics.NewDesignEvent(schema.EventName, schema.ToDictionary());
        }

        private void InitializeGA()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            InitializeGA();
        }

        public void GameAnalyticsATTListenerRestricted()
        {
            InitializeGA();
        }

        public void GameAnalyticsATTListenerDenied()
        {
            InitializeGA();
        }

        public void GameAnalyticsATTListenerAuthorized()
        {
            InitializeGA();
        }
    }
}