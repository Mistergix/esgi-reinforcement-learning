using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DevLocker.Utils;
using PGSauce.Core.PGDebugging;
using PGSauce.Unity;

namespace PGSauce.Games.IaEsgi
{
    public class SceneManager : PGMonoBehaviour
    {
        #region Public And Serialized Fields
        [SerializeField] private SceneReference gridWorldScene;
        [SerializeField] private SceneReference sokobanScene;
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        public void Awake()
        {
        }
        
        public void Start()
        {
        }
        
        public void Update()
        {
        }
        
        public void OnEnable()
        {
        }
        
        public void OnDisable()
        {
        }
        
        public void OnDestroy()
        {
        }
        
        #endregion
        #region Public Methods
        public void LoadGridWorld()
        {
           LoadScene(gridWorldScene);
        }
        public void LoadSokoban()
        {
            LoadScene(sokobanScene);
        }
        #endregion
        #region Private Methods
        private void LoadScene(SceneReference scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ScenePath);
        }
        #endregion
    }
}
