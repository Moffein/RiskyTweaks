﻿using BepInEx;
using Facepunch.Steamworks;
using R2API.Utils;
using RiskyTweaks.FireSelect;
using RiskyTweaks.Tweaks;
using System;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace RiskyTweaks
{
    [BepInDependency("com.Moffein.TeleExpansion", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.funkfrog_sipondo.sharesuite", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("zombieseatflesh7.ArtifactOfPotential", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Moffein.Heretic", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin("com.Moffein.RiskyTweaks", "RiskyTweaks", "1.7.0")]
    public class RiskyTweaks : BaseUnityPlugin
    {
        public static PluginInfo pluginInfo;

        private void Awake()
        {
            pluginInfo = Info;
            On.RoR2.Language.SetFolders += LoadLanguage.fixme;
            ModCompat.Init();

            base.gameObject.AddComponent<FireSelectManager>();
            FireSelectManager.Init(Config);

            AddToAssembly();
        }

        private void AddToAssembly()
        {
            var fixTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TweakBase)));

            foreach (var tweakType in fixTypes)
            {
                TweakBase tweak = (TweakBase)Activator.CreateInstance(tweakType);
                tweak.Init(Config);
            }
        }
    }
}
