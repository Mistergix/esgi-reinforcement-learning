using UnityEngine;

namespace PGSauce.Core.FSM.WithSo
{
    public abstract class SoActionBase : ScriptableObject
    {
        [SerializeField] private string actionName;

        public string ActionName { get => actionName; }
    }
}
