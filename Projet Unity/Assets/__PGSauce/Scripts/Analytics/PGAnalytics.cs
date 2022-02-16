using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Analytics
{
    public class PGAnalytics : MonoBehaviour
    {
        public void Init()
        {
            AnalyticsManager.Initialize(true);
        }
    }
}
