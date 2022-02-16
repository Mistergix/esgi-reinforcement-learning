using System;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.PGDebugging;
using PGSauce.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PGSauce.PGRemote.ABTest
{
    public class ABTestDebugUI : PGMonoBehaviour
    {
        [SerializeField, Min(1)] private float tapTimeThreshold = 4;
        [SerializeField, Min(5)] private int tapCount = 8;
        [SerializeField] private GameObject ui;
        [SerializeField] private PGRemoteConfig remote;
        
        private List<TapData> _tapDatas;
        private bool _openedUI;

        private void OnEnable()
        {
            _tapDatas = new List<TapData>();
            _openedUI = false;
            ui.SetActive(false);
        }

        public void Update()
        {
            if (_openedUI)
            {
                return;
            }
            
            _tapDatas = _tapDatas.Where(data => Time.time - data.TapTime < tapTimeThreshold).ToList();
            if (_tapDatas.Count >= tapCount)
            {
                ShowUI();
            }
        }
        
        public void OnTap()
        {
            PGDebug.Message($"On tap {_tapDatas.Count + 1}").Log();
            _tapDatas.Add(new TapData(Time.time));
        }

        private void ShowUI()
        {
            _openedUI = true;
            ui.SetActive(true);
            ABTestHandler.Instance.Debug = true;
        }

        public void ValidateAbTests()
        {
            _openedUI = false;
            ui.SetActive(false);
            remote.Init();
            SceneManager.LoadScene(1);
        }

        private struct TapData
        {
            public float TapTime;

            public TapData(float tapTime)
            {
                TapTime = tapTime;
            }
        }
    }
}