using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGSauce.Core.GlobalVariables
{
    public class IGlobalValue<T> : IGlobalValueScriptableObject
    {
        [SerializeField] private T value;

        public T Value { get => value; set => this.value = value; }
        
        public static implicit operator T(IGlobalValue<T> val) => val.Value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}

