using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Google.Play.AppUpdate;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using TMPro;
using UnityEngine.SceneManagement;

namespace PGSauce.Core.Patching
{
    public class InAppAndroidUpdate : MonoBehaviour
    {
        [SerializeField] private UpdateUiBase updateUi;
        private AppUpdateManager _appUpdateManager;

        private void Awake()
        {
#if ! UNITY_EDITOR
            _appUpdateManager = new AppUpdateManager();
            StartCoroutine(CheckForUpdate());
#endif
#if UNITY_EDITOR
            LoadNextScene();
#endif
        }

        private void LoadNextScene()
        {
            updateUi.LoadNextScene();
        }

        private IEnumerator CheckForUpdate()
        {
            var appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();
            updateUi.LookForUpdate();
            yield return appUpdateInfoOperation;
            if (appUpdateInfoOperation.IsSuccessful)
            {
                var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
                HandleUpdate(appUpdateInfoResult);
            }
            else
            {
                updateUi.Error(appUpdateInfoOperation.Error);
            }
        }
        
        private void HandleUpdate(AppUpdateInfo appUpdateInfoResult)
        {
            var availability = appUpdateInfoResult.UpdateAvailability;

            if (availability != UpdateAvailability.UpdateAvailable)
            {
                updateUi.NoUpdate();
                LoadNextScene();
                return;
            }
            
            var priority = appUpdateInfoResult.UpdatePriority;
            var stalenessDays = appUpdateInfoResult.ClientVersionStalenessDays;
            
            if (stalenessDays.HasValue)
            {
                PGDebug.Message($"Has not been proposed un update for {stalenessDays.Value} days").LogTodo();
            }

            var updateOptions = GetAppUpdateOptions(priority, false);
            switch (updateOptions.AppUpdateType)
            {
                case AppUpdateType.Immediate:
                    HandleImmediateUpdate(appUpdateInfoResult, updateOptions);
                    break;
                case AppUpdateType.Flexible:
                    HandleFlexibleUpdate(appUpdateInfoResult, updateOptions);
                    break;
            }
        }

        private void HandleImmediateUpdate(AppUpdateInfo appUpdateInfoResult, AppUpdateOptions updateOptions)
        {
            StartCoroutine(StartImmediateUpdate(appUpdateInfoResult, updateOptions));
        }

        private IEnumerator StartImmediateUpdate(AppUpdateInfo appUpdateInfoResult, AppUpdateOptions updateOptions)
        {
            var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfoResult, updateOptions);
            yield return startUpdateRequest;
            updateUi.Error(startUpdateRequest.Error);
        }

        private void HandleFlexibleUpdate(AppUpdateInfo appUpdateInfoResult, AppUpdateOptions updateOptions)
        {
            StartCoroutine(StarFlexibleUpdate(appUpdateInfoResult, updateOptions));
        }

        private IEnumerator StarFlexibleUpdate(AppUpdateInfo appUpdateInfoResult, AppUpdateOptions updateOptions)
        {
            var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfoResult, updateOptions);
            updateUi.StartUpdating();

            while (!startUpdateRequest.IsDone)
            {
                updateUi.Updating(startUpdateRequest);
                yield return null;
            }

            StartCoroutine(CompleteFlexibleUpdate());
        }

        private IEnumerator CompleteFlexibleUpdate()
        {
            var result = _appUpdateManager.CompleteUpdate();
            updateUi.FinishUpdate();
            yield return result;
            updateUi.Error(result.Error);
        }

        private AppUpdateOptions GetAppUpdateOptions(int priority, bool allowAssetPackDeletion )
        {
            if (priority <= 1)
            {
                return AppUpdateOptions.FlexibleAppUpdateOptions(allowAssetPackDeletion);
            }

            if (priority <= 3)
            {
                return AppUpdateOptions.FlexibleAppUpdateOptions(allowAssetPackDeletion);
            }
            
            return AppUpdateOptions.ImmediateAppUpdateOptions(allowAssetPackDeletion);
        }

    }
}
