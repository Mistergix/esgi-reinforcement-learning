using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.GlobalVariables;
using PGSauce.Core.Strings;
using UnityEngine;

namespace PGSauce.Core
{
    [CreateAssetMenu(fileName = "new Global Float", menuName = MenuPaths.MenuBase + "Global Variables/Global Float")]
    public class GlobalFloat : IGlobalValue<float>
    {
    }
}