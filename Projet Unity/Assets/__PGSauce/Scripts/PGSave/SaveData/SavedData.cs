using UnityEngine;

namespace PGSauce.Save
{
    public abstract class SavedData<T> : SavedDataBase
    {
        [SerializeField] private T defaultValue;
        public virtual string Key => name;
        public T DefaultValue => defaultValue;

        public void SaveData(T value)
        {
            CustomBeforeSave(value);
            PgSave.Instance.Save(this, value);
            CustomAfterSave(value);
        }

        public T Load()
        {
            CustomBeforeLoad();
            var value = PgSave.Instance.Load(this);
            CustomAfterLoad();
            return value;
        }
        
        protected virtual void CustomAfterSave(T value)
        {
        }

        protected virtual void CustomBeforeSave(T value)
        {
        }
        
        protected virtual void CustomBeforeLoad()
        {
        }
        
        protected virtual void CustomAfterLoad()
        {
        }
    }
}