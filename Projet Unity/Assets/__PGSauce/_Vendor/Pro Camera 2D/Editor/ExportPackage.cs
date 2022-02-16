using Com.LuisPedroFonseca.ProCamera2D;
using UnityEditor;
using UnityEngine;

public class ExportPackage : MonoBehaviour
{
    [MenuItem("ProCamera2D/ExportPackage")]
    static void ExportThisPackage()
    {
        AssetDatabase.ExportPackage(
            new string[]
            {
                "Assets/ProCamera2D",
                "Assets/Gizmos/ProCamera2D"
            },
            $"Assets/procamera2d_{ProCamera2D.Version}.unitypackage",
            ExportPackageOptions.Recurse
        );
    }
}
