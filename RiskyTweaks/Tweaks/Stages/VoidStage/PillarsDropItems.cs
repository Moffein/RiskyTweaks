using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Stages.VoidStage
{
    public class PillarsDropItems : TweakBase<PillarsDropItems>
    {
        public override string ConfigCategoryString => "Stages - Void Locus";

        public override string ConfigOptionName => "(Server-Side) Signals Drop Items";

        public override string ConfigDescriptionString => "Signals drop items for the team on completion.";

        private static Vector3 rewardPositionOffset = new Vector3(0f, 3f, 0f);
        public static float whiteChance = 45f;
        public static float greenChance = 40f;
        public static float redChance = 5f;
        public static float voidChance = 10f;

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            whiteChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier1 Chance", 45f, "Chance to drop white items.").Value;
            greenChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier2 Chance", 40f, "Chance to drop green items.").Value;
            redChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier3 Chance", 5f, "Chance to drop red items.").Value;
            voidChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Void Chance", 10f, "Chance to drop Void items.").Value;
        }

        protected override void ApplyChanges()
        {
            On.RoR2.HoldoutZoneController.OnDisable += HoldoutZoneController_OnDisable;
        }

        private void HoldoutZoneController_OnDisable(On.RoR2.HoldoutZoneController.orig_OnDisable orig, HoldoutZoneController self)
        {
            orig(self);

            if (NetworkServer.active)
            {
                SceneDef sd = RoR2.SceneCatalog.GetSceneDefForCurrentScene();
                if (sd)
                {
                    if (sd.baseSceneName.Equals("voidstage"))
                    {
                        HoldoutZoneController holdoutZone = self;
                        PickupDropTable dropTable = SelectItem();

                        PickupIndex pickupIndex = dropTable.GenerateDrop(Run.instance.bossRewardRng);
                        ItemTier tier = PickupCatalog.GetPickupDef(pickupIndex).itemTier;
                        if (pickupIndex != PickupIndex.none)
                        {
                            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);

                            int participatingPlayerCount = Run.instance.participatingPlayerCount;
                            if (participatingPlayerCount != 0 && holdoutZone.transform)
                            {
                                int num = participatingPlayerCount;

                                bool itemShareActive = false;
                                switch (tier)
                                {
                                    case ItemTier.Tier1:
                                        if (ModCompat.ShareSuiteCompat.ItemSettings.common)
                                        {
                                            num = 1;
                                        }
                                        break;
                                    case ItemTier.Tier2:
                                        if (ModCompat.ShareSuiteCompat.ItemSettings.uncommon)
                                        {
                                            num = 1;
                                        }
                                        break;
                                    case ItemTier.Tier3:
                                        if (ModCompat.ShareSuiteCompat.ItemSettings.legendary)
                                        {
                                            num = 1;
                                        }
                                        break;
                                    case ItemTier.VoidTier1:
                                    case ItemTier.VoidTier2:
                                    case ItemTier.VoidTier3:
                                    case ItemTier.VoidBoss:
                                        if (ModCompat.ShareSuiteCompat.ItemSettings.voidItem)
                                        {
                                            num = 1;
                                        }
                                        break;
                                    default: break;
                                }

                                float angle = 360f / (float)num;
                                Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

                                //Roll the rng anyways so that it performs the same with/without the config option. This code is a mess.
                                PickupPickerController.Option[] options = PickupPickerController.GenerateOptionsFromDropTable(3, dropTable, Run.instance.bossRewardRng);
                                if (options.Length > 0 && options[0].pickupIndex != PickupIndex.none) pickupDef = PickupCatalog.GetPickupDef(options[0].pickupIndex);

                                if (pickupDef != null && pickupDef.pickupIndex != PickupIndex.none)
                                {
                                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                                    {
                                        baseToken = "VOID_SIGNAL_COMPLETE_RISKYMOD",
                                        paramTokens = new string[]
                                        {
                                                "<color=#"+ColorUtility.ToHtmlStringRGB(pickupDef.baseColor)+">"+Language.GetStringFormatted(pickupDef.nameToken) + "</color>"
                                        }
                                    });
                                }

                                int k = 0;
                                while (k < num)
                                {
                                    PickupDropletController.CreatePickupDroplet(new GenericPickupController.CreatePickupInfo
                                    {
                                        pickerOptions = options,
                                        prefabOverride = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion(),
                                        rotation = Quaternion.identity,
                                        pickupIndex = PickupCatalog.FindPickupIndex(tier)
                                    }, holdoutZone.transform.position + rewardPositionOffset, vector);
                                    k++;
                                    vector = rotation * vector;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static PickupDropTable SelectItem()
        {
            Xoroshiro128Plus bossRewardRng = Run.instance.bossRewardRng;

            float total = whiteChance + greenChance + redChance + voidChance;

            if (bossRewardRng.RangeFloat(0f, total) <= whiteChance && whiteChance > 0f)//drop white
            {
                return Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier1Item.asset").WaitForCompletion();
            }
            else
            {
                total -= whiteChance;
                if (bossRewardRng.RangeFloat(0f, total) <= greenChance && greenChance > 0f)//drop green
                {
                    return Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier2Item.asset").WaitForCompletion();
                }
                else
                {
                    total -= greenChance;
                    if ((bossRewardRng.RangeFloat(0f, total) <= redChance) && redChance > 0f)
                    {
                        return Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier3Item.asset").WaitForCompletion();
                    }
                    else
                    {
                        return Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtVoidChest.asset").WaitForCompletion();
                    }
                }
            }
        }
    }
}
