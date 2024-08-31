using RiskOfOptions;
using RiskyTweaks.FireSelect;
using RoR2;
using System.Runtime.CompilerServices;

namespace RiskyTweaks
{
    internal static class ModCompat
    {
        internal static void Init()
        {
            RiskOfOptionsCompat.pluginLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
            ShareSuiteCompat.pluginLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.funkfrog_sipondo.sharesuite");
            ShareSuiteCompat.ReadSettings();
            ArtifactOfPotentialCompat.pluginLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("zombieseatflesh7.ArtifactOfPotential");

            RoR2Application.onLoad += RiskOfOptionsCompat.AddOptions;
        }

        public static class RiskOfOptionsCompat
        {
            public static bool pluginLoaded;

            internal static void AddOptions()
            {
                if (pluginLoaded) AddOptionsInternal();
            }

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            private static void AddOptionsInternal()
            {
                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(Tweaks.Survivors.Merc.M1AttackSpeed.Instance.Enabled));

                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(FireSelectManager.scrollSelection));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(FireSelectManager.nextButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(FireSelectManager.prevButton));

                if (Tweaks.Survivors.Bandit2.PrimaryAutoFire.Instance.Enabled.Value)
                {
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.enabled));
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.defaultButton));
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.spamButton));
                }

                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(EngiAutoFire.enabled));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(EngiAutoFire.autoButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(EngiAutoFire.holdButton));

                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(CaptainAutoFire.enabled));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.defaultButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.autoButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.chargedButton));
            }
        }

        public static class ShareSuiteCompat
        {
            public static bool pluginLoaded;

            internal static void ReadSettings()
            {
                if (pluginLoaded) ReadSettingsInternal();
            }

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            private static void ReadSettingsInternal()
            {
                ItemSettings.common = ShareSuite.ShareSuite.WhiteItemsShared.Value;
                ItemSettings.uncommon = ShareSuite.ShareSuite.GreenItemsShared.Value;
                ItemSettings.legendary = ShareSuite.ShareSuite.RedItemsShared.Value;
                ItemSettings.boss = ShareSuite.ShareSuite.BossItemsShared.Value;
                ItemSettings.lunar = ShareSuite.ShareSuite.LunarItemsShared.Value;
                ItemSettings.voidItem = ShareSuite.ShareSuite.VoidItemsShared.Value;
            }

            public static class ItemSettings
            {
                public static bool common = false;
                public static bool uncommon = false;
                public static bool legendary = false;
                public static bool boss = false;
                public static bool lunar = false;
                public static bool voidItem = false;
            }
        }

        public static class ArtifactOfPotentialCompat
        {
            public static bool pluginLoaded;

            public static bool IsArtifactActive()
            {
                bool isActive = false;
                if (pluginLoaded) isActive = IsArtifactActiveInternal();
                return isActive;
            }

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            private static bool IsArtifactActiveInternal()
            {
                return RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(ArtifactOfPotential.PotentialArtifact.Potential);
            }
        }
    }
}
