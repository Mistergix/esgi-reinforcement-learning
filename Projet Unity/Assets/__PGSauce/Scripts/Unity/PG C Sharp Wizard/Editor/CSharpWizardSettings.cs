using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;

namespace PGSauce.Core.CSharpWizard
{
    [CreateAssetMenu(menuName = MenuPaths.Settings + "C# Wizard Settings")]
    public class CSharpWizardSettings : SOSingleton<CSharpWizardSettings>
    {
        [SerializeField] private List<string> usings = new List<string>()
            {"UnityEngine", "System.Collections", "System.Collections.Generic"};

        [BoxGroup("Namespace"), SerializeField] private string defaultNotInGameNameSpace;
        [BoxGroup("Namespace"), ShowInInspector]
        public string DefaultGameNamespace => PGSettings.Instance.GamesNamespace;
        [BoxGroup("Namespace")]
        public string DefaultNotInGameNameSpace => defaultNotInGameNameSpace;

        public ReadOnlyCollection<string> Usings => usings.AsReadOnly();
        public ReadOnlyCollection<string> UsingsInspector => GetUsingsInspector();
        

        private ReadOnlyCollection<string> GetUsingsInspector()
        {
            var u = new List<string>(Usings) {"UnityEditor"};
            return u.AsReadOnly();
        }
    }
}
