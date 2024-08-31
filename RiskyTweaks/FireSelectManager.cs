using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks
{
    public class FireSelectManager : MonoBehaviour
    {
        public static ConfigEntry<bool> scrollSelection;
        public static ConfigEntry<KeyboardShortcut> nextButton;
        public static ConfigEntry<KeyboardShortcut> prevButton;

        public delegate void FireMode();
        public static FireMode FireModeActions;

        internal static void ReadConfig(ConfigFile config)
        {
            scrollSelection = config.Bind<bool>("Fire Select", "Scroll Selection", true, "Scroll wheel swaps between firemodes.");
            nextButton = config.Bind<KeyboardShortcut>("Fire Select", "Next Button", KeyboardShortcut.Empty, "Button to swap to next firemode.");
            prevButton = config.Bind<KeyboardShortcut>("Fire Select", "Previous Button", KeyboardShortcut.Empty, "Button to swap to previous firemode.");
        }

        private void Update()
        {
            if (FireModeActions != null) FireModeActions.Invoke();
        }
    }
}
