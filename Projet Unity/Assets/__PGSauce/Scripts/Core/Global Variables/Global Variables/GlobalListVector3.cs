using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.GlobalVariables;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.Core
{
    [CreateAssetMenu(fileName = "new Global ListVector3", menuName = MenuPaths.MenuBase + "Global Variables/Global ListVector3")]
    public class GlobalListVector3 : IGlobalValue<List<Vector3>>
    {
    }
}