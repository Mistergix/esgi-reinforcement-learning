using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using MonKey.Extensions;
using Sirenix.OdinInspector;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoStateBase : ScriptableObject
    {
        [SerializeField] private bool isNullState;
        [SerializeField] [HideIf("IsNullState")]
        private string stateName;

        public int debugIndex;

        public string StateName => IsNullState ? "NULL" : (stateName.IsNullOrEmpty() ? name : stateName);
        public bool IsNullState => isNullState;
    }
}
