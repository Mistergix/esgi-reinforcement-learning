using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.GlobalVariables;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.Core
{
    [CreateAssetMenu(fileName = "new Global Bool", menuName = MenuPaths.MenuBase + "Global Variables/Global Bool")]
    public class GlobalBool : IGlobalValue<bool>
    {
    }
}