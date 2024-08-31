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
    public class ShrineCombatItems : TweakBase<ShrineCombatItems>
    {
        public override string ConfigCategoryString => "Interactables - Shrine of Combat Items";

        public override string ConfigOptionName => "(Server-Side) Shrine of Combat Items";

        public override string ConfigDescriptionString => "Shrine of Combat gives less money, but drops items for the team on completion.";

        public static int maxPerStage = 3;
        public static float whiteChance = 100f;
        public static float greenChance = 0f;
        public static float redChance = 0f;
        private static Vector3 rewardPositionOffset = new Vector3(0f, 6f, 0f);

        private static InteractableSpawnCard shrineCombat = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineCombat/iscShrineCombat.asset").WaitForCompletion();
        private static InteractableSpawnCard shrineCombatSandy = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineCombat/iscShrineCombatSandy.asset").WaitForCompletion();
        private static InteractableSpawnCard shrineCombatSnowy = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineCombat/iscShrineCombatSnowy.asset").WaitForCompletion();
        private static int shrineCombatCostInitial;
        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            maxPerStage = config.Bind<int>(ConfigCategoryString, ConfigOptionName + " - Max Per Stage", 3, "Maximum that can spawn on 1 stage. Set to -1 for no limit.").Value;
            whiteChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier1 Chance", 100f, "Chance to drop white items.").Value;
            greenChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier2 Chance", 0f, "Chance to drop green items.").Value;
            redChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier3 Chance", 0f, "Chance to drop red items.").Value;
        }

        protected override void ApplyChanges()
        {
            //Scale cost since this drops items for everyone
            RoR2.RoR2Application.onLoad += GetInitialShrineCost;
            On.RoR2.SceneDirector.Start += SceneDirector_Start;
            On.RoR2.ShrineCombatBehavior.Awake += ShrineCombatBehavior_Awake;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer += ShrineCombatBehavior_OnDefeatedServer;
        }

        private void ShrineCombatBehavior_OnDefeatedServer(On.RoR2.ShrineCombatBehavior.orig_OnDefeatedServer orig, ShrineCombatBehavior self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                ItemTier tier = ItemTier.NoTier;
                PickupIndex pickupIndex = SelectItem();
                PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                if (pickupIndex != PickupIndex.none)
                {
                    int participatingPlayerCount = Run.instance.participatingPlayerCount;
                    if (participatingPlayerCount != 0 && self.transform)
                    {
                        int num = participatingPlayerCount;

                        PickupDropTable dropTable;
                        tier = pickupDef.itemTier;
                        switch (tier)
                        {
                            case ItemTier.Tier2:
                                dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier2Item.asset").WaitForCompletion();
                                if (ModCompat.ShareSuiteCompat.ItemSettings.uncommon) num = 1;
                                break;
                            case ItemTier.Tier3:
                                dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier3Item.asset").WaitForCompletion();
                                if (ModCompat.ShareSuiteCompat.ItemSettings.legendary) num = 1;
                                break;
                            default:
                                dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier1Item.asset").WaitForCompletion();
                                if (ModCompat.ShareSuiteCompat.ItemSettings.common) num = 1;
                                break;
                        }

                        float angle = 360f / (float)num;
                        Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

                        //Roll the rng anyways so that it performs the same with/without the config option. This code is a mess.
                        PickupPickerController.Option[] options = PickupPickerController.GenerateOptionsFromDropTable(3, dropTable, Run.instance.bossRewardRng);
                        if (options.Length > 0 && options[0].pickupIndex != PickupIndex.none)
                        {
                            pickupDef = PickupCatalog.GetPickupDef(options[0].pickupIndex);
                            pickupIndex = pickupDef.pickupIndex;
                        }

                        int k = 0;
                        while (k < num)
                        {
                            if (!ModCompat.ArtifactOfPotentialCompat.IsArtifactActive())
                            {
                                PickupDropletController.CreatePickupDroplet(pickupDef.pickupIndex, self.transform.position + rewardPositionOffset, vector);
                            }
                            else
                            {
                                PickupDropletController.CreatePickupDroplet(new GenericPickupController.CreatePickupInfo
                                {
                                    pickerOptions = options,
                                    prefabOverride = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion(),
                                    rotation = Quaternion.identity,
                                    pickupIndex = PickupCatalog.FindPickupIndex(tier)
                                }, self.transform.position + rewardPositionOffset, vector);
                            }
                            k++;
                            vector = rotation * vector;
                        }

                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                        {
                            baseToken = "SHRINE_COMBAT_END_TRIAL_RISKYMOD",
                            paramTokens = new string[]
                            {
                                    "<color=#"+ColorUtility.ToHtmlStringRGB(pickupDef.baseColor)+">"+Language.GetStringFormatted(pickupDef.nameToken) + "</color>"
                            }
                        });
                    }
                }
            }
        }

        private void ShrineCombatBehavior_Awake(On.RoR2.ShrineCombatBehavior.orig_Awake orig, ShrineCombatBehavior self)
        {
            orig(self);

            //Adjust money rewards, and make it so that it only triggers twice. (Set to reduced, then set to almost normal)
            //bad way to do this
            if (self.combatDirector)
            {
                if (self.combatDirector.expRewardCoefficient == 0.2f)
                {
                    self.combatDirector.expRewardCoefficient = 0.067f;
                }
                else if (self.combatDirector.expRewardCoefficient == 0.067f)
                {
                    self.combatDirector.expRewardCoefficient = 0.199f;
                }
            }
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (NetworkServer.active && Run.instance)
            {
                int playerCount = Mathf.Max(Run.instance.participatingPlayerCount, 1);
                float costMult = 1f + 0.5f * (playerCount - 1);
                SetShrineCombatCost(costMult);
            }
            orig(self);
        }

        private void GetInitialShrineCost()
        {
            shrineCombatCostInitial = shrineCombat.directorCreditCost;

            if (maxPerStage >= 0)
            {
                shrineCombat.maxSpawnsPerStage = maxPerStage;
                shrineCombatSandy.maxSpawnsPerStage = maxPerStage;
                shrineCombatSnowy.maxSpawnsPerStage = maxPerStage;
            }
        }

        private static void SetShrineCombatCost(float costMultiplier)
        {
            shrineCombat.directorCreditCost = Mathf.FloorToInt(shrineCombatCostInitial * costMultiplier);
            shrineCombatSandy.directorCreditCost = Mathf.FloorToInt(shrineCombatCostInitial * costMultiplier);
            shrineCombatSnowy.directorCreditCost = Mathf.FloorToInt(shrineCombatCostInitial * costMultiplier);
        }
        private static PickupIndex SelectItem()
        {
            List<PickupIndex> list;
            Xoroshiro128Plus treasureRng = Run.instance.treasureRng;
            PickupIndex selectedPickup = PickupIndex.none;

            float total = whiteChance + greenChance + redChance;

            if (treasureRng.RangeFloat(0f, total) <= whiteChance)//drop white
            {
                list = Run.instance.availableTier1DropList;
            }
            else
            {
                total -= whiteChance;
                if (treasureRng.RangeFloat(0f, total) <= greenChance)//drop green
                {
                    list = Run.instance.availableTier2DropList;
                }
                else
                {
                    total -= greenChance;
                    list = Run.instance.availableTier3DropList;
                }
            }
            if (list.Count > 0)
            {
                selectedPickup = treasureRng.NextElementUniform<PickupIndex>(list);
            }
            return selectedPickup;
        }
    }
}
