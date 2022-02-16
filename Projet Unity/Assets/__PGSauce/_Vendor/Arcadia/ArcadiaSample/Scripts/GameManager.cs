using GameTroopers.UI;
using UnityEngine;

namespace Arcadia.Sample
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;
        
        private void Start()
        {
            m_menuManager.Initialize();
            m_menuManager.LoadGroup(GroupName);
    
            ShowTopBar();
            ShowMainMenu();
        }

        private static string GroupName => "Group1";

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
    
            DontDestroyOnLoad(gameObject);
        }
    
        /// <summary>
        /// Main menu
        /// </summary>
        public void ShowMainMenu()
        {
            if (!m_menuManager.GetMenu<TopBarMenu>(GroupName).IsActive)
            {
                ShowTopBar();
            }
    
            m_menuManager.ShowMenu<MainMenu>(GroupName);
        }
        
        /// <summary>
        /// Inventory menu
        /// </summary>
        public void ShowInventory()
        {
            HideTopBar();
            m_menuManager.ShowMenu<InventoryMenu>(GroupName);
        }
        
        /// <summary>
        /// Top bar menu
        /// </summary>
        public void ShowTopBar()
        {
            m_menuManager.ShowMenu<TopBarMenu>(GroupName);
        }
    
        public void HideTopBar()
        {
            m_menuManager.HideMenu<TopBarMenu>(GroupName);
        }
    
        /// <summary>
        /// Shop menu
        /// </summary>
        public void ShowShop()
        {
            m_menuManager.ShowMenu<ShopMenu>(GroupName);
        }
    
        /// <summary>
        /// Settings menu
        /// </summary>
        public void ShowSettings()
        {
            m_menuManager.ShowMenu<SettingsMenu>(GroupName);
        }
        
        /// <summary>
        /// Popup menu
        /// </summary>
        public void ShowPopup()
        {
            m_menuManager.ShowMenu<PopupMenu>(GroupName);
        }
    
        /// <summary>
        /// Back event
        /// </summary>
        public void BackEvent()
        {
            m_menuManager.GoBack();
        }
    
        [SerializeField] MenuManager m_menuManager;
    }

}

