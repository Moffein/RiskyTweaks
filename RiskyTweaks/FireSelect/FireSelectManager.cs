using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.FireSelect
{
    public class FireSelectManager : MonoBehaviour
    {
        public static ConfigEntry<bool> scrollSelection;
        public static ConfigEntry<KeyboardShortcut> nextButton;
        public static ConfigEntry<KeyboardShortcut> prevButton;

        public delegate void FireMode();
        public static FireMode FireModeActions;

        private static void ReadConfig(ConfigFile config)
        {
            scrollSelection = config.Bind("Fire Select", "Scroll Selection", true, "Scroll wheel swaps between firemodes.");
            nextButton = config.Bind("Fire Select", "Next Button", KeyboardShortcut.Empty, "Button to swap to next firemode.");
            prevButton = config.Bind("Fire Select", "Previous Button", KeyboardShortcut.Empty, "Button to swap to previous firemode.");
        }

        internal static void Init(ConfigFile config)
        {
            ReadConfig(config);

            CaptainAutoFire.Init(config);
            EngiAutoFire.Init(config);
        }

        private void Update()
        {
            if (FireModeActions != null) FireModeActions.Invoke();
        }
    }
}
