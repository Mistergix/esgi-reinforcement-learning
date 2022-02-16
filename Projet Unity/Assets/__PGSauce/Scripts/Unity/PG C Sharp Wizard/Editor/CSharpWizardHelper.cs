using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pinwheel.CsharpWizard;

namespace PGSauce.Core.CSharpWizard
{
    public static class CSharpWizardHelper
    {
        private static string DefaultGameNameSpace => CSharpWizardSettings.Instance.DefaultGameNamespace;
        private static string DefaultNotInGameNameSpace => CSharpWizardSettings.Instance.DefaultNotInGameNameSpace;
        
        public static ClassWizard.UsingNode GetUsingNode()
        {
            var settings = CSharpWizardSettings.Instance;
            var usings = settings.Usings;
            return GetUsingNode(usings);
        }
        
        public static ClassWizard.UsingNode GetUsingEditorNode()
        {
            var settings = CSharpWizardSettings.Instance;
            var usings = settings.UsingsInspector;
            return GetUsingNode(usings);
        }
        
        public static string GetDefaultNamespace(string directory)
        {
            return PGAssets.IsInsidePgSauce(directory) ? DefaultNotInGameNameSpace : DefaultGameNameSpace;
        }
        
        private static ClassWizard.UsingNode GetUsingNode(ReadOnlyCollection<string> usings)
        {
            var un = new ClassWizard.UsingNode();
            foreach (var usingString in usings)
            {
                un.Using(usingString);
            }

            return un;
        }

        public static List<string> GetCustomMonoBehaviourCode()
        {
            var code = new List<string>();
            code.Add(GetCompleteRegion("Public And Serialized Fields"));
            code.Add(GetCompleteRegion("Private Fields"));
            code.Add(GetCompleteRegion("Properties"));
            code.Add(GetRegion("Unity Functions"));
            code.Add(ClassWizard.UnityMessage.AWAKE);
            code.Add(ClassWizard.UnityMessage.START);
            code.Add(ClassWizard.UnityMessage.UPDATE);
            code.Add(ClassWizard.UnityMessage.ON_ENABLE);
            code.Add(ClassWizard.UnityMessage.ON_DISABLE);
            code.Add(ClassWizard.UnityMessage.ON_DESTROY);
            code.Add(EndRegion);
            code.Add(GetCompleteRegion("Public Methods"));
            code.Add(GetCompleteRegion("Private Methods"));

            return code;
        }

        private static string GetCompleteRegion(string regionName)
        {
            return $"{GetRegion(regionName)}{EndRegion}";
        }

        private static string GetRegion(string regionName)
        {
            return $"#region {regionName}\n";
        }
        private static string EndRegion => "#endregion\n\n";
    }
}