using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using PGSauce.Core.PGDebugging;
using UnityEditor;
#endif

using UnityEngine;
#pragma warning disable 162

namespace PGSauce.Core
{
    public static class PGAssets
    {
        public static string AssetsDirPath => Application.dataPath;
        public static string ProjectDirPath => Directory.GetParent(AssetsDirPath).FullName;
        
        public static string GetPathUnderMouse(bool sanitized = true)
        {
#if UNITY_EDITOR
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            if(sanitized) {folderPath = SanitizePath(folderPath);}
            return folderPath;
#endif
            return "";
        }

        private static string SanitizePath(string folderPath)
        {
#if UNITY_EDITOR
            if (folderPath.Contains("."))
                folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
            folderPath = folderPath.Replace("Assets/", "");
            folderPath = folderPath.Replace("Assets", "");
            return folderPath;
#endif
            return "";
        }

        public static string GetAssetPath(Object obj, bool sanitized = true)
        {
#if UNITY_EDITOR
            var folderPath = AssetDatabase.GetAssetPath(obj);
            if(sanitized) {folderPath = SanitizePath(folderPath);}
            return folderPath;
#endif
            return "";
        }
        
        public static void AbstractGenerateOneFile(string result, string intoPath, string fileName)
        {
#if UNITY_EDITOR
            if (!Directory.Exists(intoPath))
            {
                Directory.CreateDirectory(intoPath);
            }
            
            intoPath = Path.Combine(intoPath, fileName);
            
            PGDebug.Message($"Generation path is {intoPath}").Log();
            
            File.WriteAllText(intoPath, result);
#endif
        }
        
        public delegate string Format(string value, int index);
        public static string AbstractFormatting(IReadOnlyList<string> values, Format formatter)
        {
#if UNITY_EDITOR
            var sb = new StringBuilder();
            
            for (var i = 0; i < values.Count; i++)
            {
                sb.Append(formatter(values[i], i)) ;
            }
            
            return sb.ToString();
#endif
            return "";
        }

        public static void SaveAssets()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            return;
        }

        public static bool IsInsidePgSauce(string localSelectedPath)
        {
#if UNITY_EDITOR
            return localSelectedPath.Contains("__PGSauce");
#endif
            return false;
        }
        
        public static T[] GetAllInstances<T>(string filter) where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:"+ filter);  //FindAssets uses tags check documentation for more info
            var a = new T[guids.Length];
            for(var i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
 
        }
    }
}