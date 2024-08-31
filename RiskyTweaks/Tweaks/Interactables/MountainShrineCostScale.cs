using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Interactables
{
    public class MountainShrineCostScale : TweakBase
    {
        public override string ConfigCategoryString => "Interactables";

        public override string ConfigOptionName => "(Server-Side) Mountain Shrine Cost Scaling";

        public override string ConfigDescriptionString => "Mountain Shrine director cost increases with playercount.";

        public static int maxPerStage = -1;
        private static InteractableSpawnCard shrineBoss = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineBoss/iscShrineBoss.asset").WaitForCompletion();
        private static InteractableSpawnCard shrineBossSandy = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineBoss/iscShrineBossSandy.asset").WaitForCompletion();
        private static InteractableSpawnCard shrineBossSnowy = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineBoss/iscShrineBossSnowy.asset").WaitForCompletion();

        private static int shrineBossCostInitial;

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            maxPerStage = config.Bind<int>(ConfigCategoryString, ConfigOptionName + " - Max Per Stage", -1, "Maximum that can spawn on 1 stage. Set to -1 for no limit.").Value;
        }

        protected override void ApplyChanges()
        {
            RoR2.RoR2Application.onLoad += GetInitialShrineCost;
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (NetworkServer.active && Run.instance)
            {
                int playerCount = Mathf.Max(Run.instance.participatingPlayerCount, 1);
                float costMult = 1f + 0.5f * (playerCount - 1);
                SetShrineBossCost(costMult);
            }
            orig(self);
        }

        private void GetInitialShrineCost()
        {
            shrineBossCostInitial = shrineBoss.directorCreditCost;

            if(maxPerStage >= 0)
            {
                shrineBoss.maxSpawnsPerStage = maxPerStage;
                shrineBossSandy.maxSpawnsPerStage = maxPerStage;
                shrineBossSnowy.maxSpawnsPerStage = maxPerStage;
            }
        }

        private static void SetShrineBossCost(float costMultiplier)
        {
            shrineBoss.directorCreditCost = Mathf.FloorToInt(shrineBossCostInitial * costMultiplier);
            shrineBossSandy.directorCreditCost = Mathf.FloorToInt(shrineBossCostInitial * costMultiplier);
            shrineBossSnowy.directorCreditCost = Mathf.FloorToInt(shrineBossCostInitial * costMultiplier);
        }
    }
}
