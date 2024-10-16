using BepInEx.Configuration;
using EntityStates.MoonElevator;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Stages.Moon
{
    public class PillarsDropItems : TweakBase<PillarsDropItems>
    {
        public override string ConfigCategoryString => "Stages - Commencement";

        public override string ConfigOptionName => "(Server-Side) Pillars Drop Items";

        public override string ConfigDescriptionString => "Pillars drop items for the team on completion.";

        private static Vector3 rewardPositionOffset = new Vector3(0f, 3f, 0f);
        public static float whiteChance = 50f;
        public static float greenChance = 40f;
        public static float redChance = 10f;
        public static float lunarChance = 0f;

        public static float pearlOverwriteChance = 15f;

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            whiteChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier1 Chance", 50f, "Chance to drop white items.").Value;
            greenChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier2 Chance", 40f, "Chance to drop green items.").Value;
            redChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Tier3 Chance", 10f, "Chance to drop red items.").Value;
            lunarChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Lunar Chance", 0f, "Chance to drop lunar items.").Value;
            pearlOverwriteChance = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Pearl Overwrite Chance", 15f, "Chance for non-Tier3 items to be rerolled into a Pearl.").Value;
        }

        protected override void ApplyChanges()
        {
            //Prevent pillars from deactivating
            On.RoR2.MoonBatteryMissionController.OnBatteryCharged += MoonBatteryMissionController_OnBatteryCharged;

        }

        //Do note that orig(self) isn't called here. The method is being completely overwritten.
        private void MoonBatteryMissionController_OnBatteryCharged(On.RoR2.MoonBatteryMissionController.orig_OnBatteryCharged orig, RoR2.MoonBatteryMissionController self, RoR2.HoldoutZoneController holdoutZone)
        {
            self.Network_numChargedBatteries = self._numChargedBatteries + 1;
            if (self._numChargedBatteries >= self._numRequiredBatteries && NetworkServer.active)
            {
                for (int k = 0; k < self.elevatorStateMachines.Length; k++)
                {
                    self.elevatorStateMachines[k].SetNextState(new InactiveToReady());
                }
            }

            if (!NetworkServer.active) return;
            PickupIndex pickupIndex = SelectItem();
            ItemTier tier = PickupCatalog.GetPickupDef(pickupIndex).itemTier;
            if (pickupIndex != PickupIndex.none)
            {
                string pillarName = "Pillar";

                PurchaseInteraction pu = holdoutZone.gameObject.GetComponent<PurchaseInteraction>();
                if (pu)
                {
                    pillarName = pu.GetDisplayName();
                }

                PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);

                int participatingPlayerCount = Run.instance.participatingPlayerCount;
                if (participatingPlayerCount != 0 && holdoutZone.transform)
                {
                    int num = participatingPlayerCount;

                    PickupDropTable dropTable;
                    bool itemShareActive = false;
                    switch (tier)
                    {
                        case ItemTier.Tier2:
                            if (ModCompat.ShareSuiteCompat.ItemSettings.uncommon)
                            {
                                num = 1;
                                itemShareActive = true;
                            }
                            dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier2Item.asset").WaitForCompletion();
                            break;
                        case ItemTier.Tier3:
                            if (ModCompat.ShareSuiteCompat.ItemSettings.legendary)
                            {
                                num = 1;
                                itemShareActive = true;
                            }
                            dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier3Item.asset").WaitForCompletion();
                            break;
                        case ItemTier.Lunar:
                            if (ModCompat.ShareSuiteCompat.ItemSettings.lunar)
                            {
                                num = 1;
                                itemShareActive = true;
                            }
                            dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/LunarChest/dtLunarChest.asset").WaitForCompletion();
                            break;
                        default:
                            if (ModCompat.ShareSuiteCompat.ItemSettings.common)
                            {
                                num = 1;
                                itemShareActive = true;
                            }
                            dropTable = Addressables.LoadAssetAsync<PickupDropTable>("RoR2/Base/Common/dtTier1Item.asset").WaitForCompletion();
                            break;
                    }

                    float angle = 360f / (float)num;
                    Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);


                    //Roll the rng anyways so that it performs the same with/without the config option. This code is a mess.
                    PickupPickerController.Option[] options = PickupPickerController.GenerateOptionsFromDropTable(3, dropTable, Run.instance.bossRewardRng);
                    if (options.Length > 0 && options[0].pickupIndex != PickupIndex.none) pickupDef = PickupCatalog.GetPickupDef(options[0].pickupIndex);

                    if ((pickupDef != null && pickupDef.pickupIndex != PickupIndex.none))
                    {
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                        {
                            baseToken = "MOON_PILLAR_COMPLETE_RISKYMOD",
                            paramTokens = new string[]
                            {
                                pillarName,
                                "<color=#"+ColorUtility.ToHtmlStringRGB(pickupDef.baseColor)+">"+Language.GetStringFormatted(pickupDef.nameToken) + "</color>"
                            }
                        });
                    }

                    int k = 0;
                    while (k < num)
                    {
                        PickupIndex pickupOverwrite = PickupIndex.none;
                        bool overwritePickup = false;
                        if (tier != ItemTier.Tier3 && !itemShareActive && !ModCompat.ShareSuiteCompat.ItemSettings.boss)
                        {
                            float pearlChance = pearlOverwriteChance;
                            float total = pearlChance;
                            if (Run.instance.bossRewardRng.RangeFloat(0f, 100f) < pearlChance)
                            {
                                pickupOverwrite = SelectPearl();
                            }

                            overwritePickup = !(pickupOverwrite == PickupIndex.none);
                        }

                        if (!overwritePickup)
                        {
                            if (!ModCompat.ArtifactOfPotentialCompat.IsArtifactActive())
                            {
                                PickupDropletController.CreatePickupDroplet(pickupDef.pickupIndex, holdoutZone.transform.position + rewardPositionOffset, vector);
                            }
                            else
                            {
                                PickupDropletController.CreatePickupDroplet(new GenericPickupController.CreatePickupInfo
                                {
                                    pickerOptions = options,
                                    prefabOverride = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion(),
                                    rotation = Quaternion.identity,
                                    pickupIndex = PickupCatalog.FindPickupIndex(tier)
                                }, holdoutZone.transform.position + rewardPositionOffset, vector);
                            }
                        }
                        else
                        {
                            PickupDropletController.CreatePickupDroplet(pickupOverwrite, holdoutZone.transform.position + rewardPositionOffset, vector);
                        }
                        k++;
                        vector = rotation * vector;
                    }
                }
            }
        }

        private static PickupIndex SelectPearl()
        {
            PickupIndex pearlIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.Pearl.itemIndex);
            PickupIndex shinyPearlIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ShinyPearl.itemIndex);
            bool pearlAvailable = pearlIndex != PickupIndex.none && Run.instance.IsItemAvailable(RoR2Content.Items.Pearl.itemIndex);
            bool shinyPearlAvailable = shinyPearlIndex != PickupIndex.none && Run.instance.IsItemAvailable(RoR2Content.Items.ShinyPearl.itemIndex);

            PickupIndex toReturn = PickupIndex.none;
            if (pearlAvailable && shinyPearlAvailable)
            {
                toReturn = pearlIndex;
                if (Run.instance.bossRewardRng.RangeFloat(0f, 100f) <= 20f)
                {
                    toReturn = shinyPearlIndex;
                }
            }
            else
            {
                if (pearlAvailable)
                {
                    toReturn = pearlIndex;
                }
                else if (shinyPearlAvailable)
                {
                    toReturn = shinyPearlIndex;
                }
            }
            return toReturn;
        }

        //Yellow Chance is handled after selecting item
        private static PickupIndex SelectItem()
        {
            List<PickupIndex> list;
            Xoroshiro128Plus bossRewardRng = Run.instance.bossRewardRng;
            PickupIndex selectedPickup = PickupIndex.none;

            float total = whiteChance + greenChance + redChance + lunarChance;

            if (bossRewardRng.RangeFloat(0f, total) <= whiteChance && whiteChance > 0)//drop white
            {
                list = Run.instance.availableTier1DropList;
            }
            else
            {
                total -= whiteChance;
                if (bossRewardRng.RangeFloat(0f, total) <= greenChance && greenChance > 0)//drop green
                {
                    list = Run.instance.availableTier2DropList;
                }
                else
                {
                    total -= greenChance;
                    if (bossRewardRng.RangeFloat(0f, total) <= redChance && redChance > 0)
                    {
                        list = Run.instance.availableTier3DropList;
                    }
                    else
                    {
                        list = Run.instance.availableLunarCombinedDropList;
                    }
                }
            }
            if (list.Count > 0)
            {
                selectedPickup = bossRewardRng.NextElementUniform<PickupIndex>(list);
            }
            return selectedPickup;
        }
    }
}