using UnityEngine;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoDecisionBase : ScriptableObject
    {
        public string DecisionName => name;
    }
}
