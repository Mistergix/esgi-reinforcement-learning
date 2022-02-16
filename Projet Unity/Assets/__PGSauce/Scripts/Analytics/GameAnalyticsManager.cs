using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;
using UnityEngine;

namespace PGSauce.Analytics
{
    [CreateAssetMenu(menuName = MenuPaths.Settings + "Game Analytics Manager")]
    public class GameAnalyticsManager : SOSingleton<GameAnalyticsManager>
    {
        public string Email => "bigcattostudio@gmail.com";
        public string Password => "1d^D9r$X6ZQ5";
    }
}