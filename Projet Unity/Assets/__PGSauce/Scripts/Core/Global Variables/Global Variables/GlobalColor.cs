using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.GlobalVariables;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.Core
{
    [CreateAssetMenu(fileName = "new Global Color", menuName = MenuPaths.MenuBase + "Global Variables/Global Color")]
    public class GlobalColor : IGlobalValue<Color>
    {
    }
}