using PGSauce.Core.Strings;
using UnityEngine;
using PGSauce.Core.Utilities;

namespace PGSauce.Core.PGDebugging
{
    [CreateAssetMenu(menuName = MenuPaths.Settings + "Debug/Settings")]
    public class PGDebugSettings : SOSingleton<PGDebugSettings>
    {
        public enum PrefixType { ClassName, MethodName, NoPrefix }
        
        public Color defaultLogColor = PGColors.Purple;
        public Color defaultLogErrorColor = PGColors.Redish;
        public Color defaultLogWarningColor = PGColors.Yellowish;
        public PrefixType defaultPrefixType = PrefixType.ClassName;
        [Min(1)] public int textSize = 14;
    }
}
