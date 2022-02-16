using GameTroopers.UI;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PGSauce.UI
{
    public abstract class PGMenuManager<T> : MonoSingleton<T> where T : MonoSingletonBase
    {
        [SerializeField] private MenuManager menuManager;
        
        protected void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        protected void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        protected abstract bool ShouldLoadMenu(Scene scene);
        protected abstract string GroupName { get; }

        protected MenuManager ArcadiaMenuManager => menuManager;

        protected abstract void ShowOnSceneLoadMenu();
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!ArcadiaMenuManager.IsInitialized)
            {
                ArcadiaMenuManager.Initialize();
            }
            
            TryLoadMenuGroup(false);
            var shouldLoad = ShouldLoadMenu(scene);
            if (!shouldLoad)
            {
                return;
            }
            
            TryLoadMenuGroup(true);
            
            ShowOnSceneLoadMenu();
        }

        protected void TryLoadMenuGroup(bool load)
        {
            if (load)
            {
                if (!ArcadiaMenuManager.IsGroupLoaded(GroupName))
                {
                    ArcadiaMenuManager.LoadGroup(GroupName);
                }
            }
            else
            {
                ArcadiaMenuManager.HideAll(false);
            }
        }

        public TMenu ShowMenu<TMenu>() where TMenu : Menu
        {
            var menuName = typeof(TMenu).ToString().RemoveNamespace();
            return ArcadiaMenuManager.ShowMenu<TMenu>(GroupName);
        }

        public TMenu GetMenu<TMenu>() where TMenu : Menu
        {
            return ArcadiaMenuManager.GetMenu<TMenu>(GroupName);
        }

        public void HideMenu<TMenu>() where TMenu : Menu
        {
            ArcadiaMenuManager.HideMenu<TMenu>(GroupName);
        }
    }
}