﻿using RiskOfOptions;
using RiskyTweaks.FireSelect;
using RiskyTweaks.Tweaks.Survivors.Mage;
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
            TeleExpansionCompat.pluginLoaded = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.TeleExpansion");

            RoR2Application.onLoad += RiskOfOptionsCompat.AddOptions;
            RoR2Application.onLoad += RiskyArtifactsCompat.InitArtifactDefs;
        }

        public static class TeleExpansionCompat
        {
            public static bool pluginLoaded;
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
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.Enabled));
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.defaultButton));
                    ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode.spamButton));
                }

                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(EngiAutoFire.Enabled));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(EngiAutoFire.autoButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(EngiAutoFire.holdButton));

                ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(CaptainAutoFire.Enabled));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.defaultButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.autoButton));
                ModSettingsManager.AddOption(new RiskOfOptions.Options.KeyBindOption(CaptainAutoFire.chargedButton));

                RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(IonSurgeMoveScaling.Instance.Enabled));
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

        public static class RiskyArtifactsCompat
        {
            public static bool pluginLoaded;

            internal static class ArtifactDefs
            {
                public static ArtifactDef Primacy;
            }

            internal static void InitArtifactDefs()
            {
                ArtifactDefs.Primacy = ArtifactCatalog.FindArtifactDef("RiskyArtifactOfPrimacy");
            }

            public static bool IsPrimacyActive()
            {
                return ArtifactDefs.Primacy != null && RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(ArtifactDefs.Primacy);
            }
        }
    }
}
