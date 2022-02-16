using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Core.Utilities
{
    public abstract class Selector<T> : MonoBehaviour
    {
        public T CurrentObject => Objects[CurrentIndex];

        protected abstract List<T> Objects
        {
            get;
            set;
        }

        private void OnEnable()
        {
            OnAwake();
        }

        protected abstract bool Cyclic { get; }

        public int CurrentIndex { get; protected set; }

        protected virtual void OnPreviousObject(){}
        protected virtual void OnNextObject(){}

        protected virtual void OnObjectChanged(){}

        protected abstract void OnAwake();

        public void NextObject()
        {
            CurrentIndex++;
            if (Cyclic)
            {
                CurrentIndex %= Objects.Count;
            }
            else
            {
                CurrentIndex = Mathf.Clamp(CurrentIndex,0, Objects.Count - 1);
            }
            
            OnNextObject();
            OnObjectChanged();
        }

        public void PreviousObject()
        {
            CurrentIndex--;
            if (Cyclic)
            {
                if (CurrentIndex < 0)
                {
                    CurrentIndex += Objects.Count;
                }
            }
            else
            {
                CurrentIndex = Mathf.Clamp(CurrentIndex,0, Objects.Count - 1);
            }
            
            OnPreviousObject();
            OnObjectChanged();
        }

    }
}
