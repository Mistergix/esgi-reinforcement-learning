
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Strings;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace PGSauce.Core
{
    public class ItemInspector
    {
#if UNITY_EDITOR
        [MenuItem(MenuPaths.SelectObject + "Select PG Settings")]
        public static void SelectPgSettings()
        {
            OdinEditorWindow.InspectObject(PGSettings.Instance).Show();
        }

        [MenuItem(MenuPaths.SelectObject + "Select PG Debug Settings")]
        public static void SelectPgDebugSettings()
        {
            OdinEditorWindow.InspectObject(PGDebugSettings.Instance).Show();
        }
        
        [MenuItem(MenuPaths.SelectObject + "Select Strings Generator")]
        public static void SelectStringsGenerator()
        {
            OdinEditorWindow.InspectObject(StringsGenerator.Instance).Show();
        }
        
        [MenuItem(MenuPaths.SelectObject + "Select Text Template Master")]
        public static void SelectTextTemplateMaster()
        {
            OdinEditorWindow.InspectObject(TextTemplateMasterSO.Instance).Show();
        }
#endif
    }
}