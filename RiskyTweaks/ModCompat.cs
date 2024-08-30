using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks
{
    internal static class ModCompat
    {
        internal static void Init()
        {
            RiskOfOptionsCompat.pluginLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
        }

        public static class RiskOfOptionsCompat
        {
            public static bool pluginLoaded;
        }
    }
}
