using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameTroopers.UI;
using PGSauce.Core.PGDebugging;
using PGSauce.Unity;

namespace PGSauce.Core.Utilities
{
    public class Clearer : PGMonoBehaviour, IOnMenuShowStarted, IOnMenuFocused
    {
        private enum DestroyMethod
        {
            SetActive,
            Destroy
        }
        
        #region Public And Serialized Fields

        [SerializeField] private BoolProvider destroyIf;
        [SerializeField] private bool invert;
        [SerializeField] private DestroyMethod destroyMethod = DestroyMethod.SetActive;
        [SerializeField] private GameObject target;
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        public void Awake()
        {
            TryDestroy();
        }

        #endregion
        #region Public Methods
        public void OnMenuShowStarted()
        {
            TryDestroy();
        }
        public void OnMenuFocused()
        {
            TryDestroy();
        }
        #endregion
        #region Private Methods
        private void TryDestroy()
        {
            var destroy = destroyIf.GetValue();
            if (invert)
            {
                destroy = !destroy;
            }

            if (target == null)
            {
                target = gameObject;
            }

            if (destroy)
            {
                switch (destroyMethod)
                {
                    case DestroyMethod.SetActive:
                        target.SetActive(false);
                        break;
                    case DestroyMethod.Destroy:
                        Destroy(target);
                        break;
                    default:
                        PGDebug.Message("No destroy method provided").LogWarning();
                        break;
                }
            }
        }
        #endregion
    }
}
