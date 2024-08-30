using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.ObjectModel;

namespace RiskyTweaks.Tweaks.Items.Warbanner
{
    public class SpawnInMorePlaces : TweakBase<SpawnInMorePlaces>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Warbanner - Spawn In More Places";

        public override string ConfigDescriptionString => "Warbanner spawns in Mithrix Phase 1 and Simulacrum.";

        private GameObject warbannerObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/WarbannerWard");

        protected override void ApplyChanges()
        {
            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += Phase1_OnEnter;
            On.EntityStates.InfiniteTowerSafeWard.Active.OnEnter += Active_OnEnter;
        }

        private void Active_OnEnter(On.EntityStates.InfiniteTowerSafeWard.Active.orig_OnEnter orig, EntityStates.InfiniteTowerSafeWard.Active self)
        {
            orig(self);
            SpawnBanners();
        }

        private void Phase1_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase1.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase1 self)
        {
            orig(self);
            SpawnBanners();
        }

        private void SpawnBanner(CharacterBody body)
        {
            if (body.inventory && body.teamComponent && body.teamComponent.teamIndex == TeamIndex.Player)
            {
                int itemCount = body.inventory.GetItemCount(RoR2Content.Items.WardOnLevel);
                if (itemCount > 0)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(warbannerObject, body.transform.position, Quaternion.identity);
                    gameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                    gameObject.GetComponent<BuffWard>().Networkradius = 8f + 8f * (float)itemCount;
                    NetworkServer.Spawn(gameObject);
                }
            }
        }

        private void SpawnBanners()
        {
            //Taken from TeleporterInteraction
            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
            for (int j = 0; j < teamMembers.Count; j++)
            {
                TeamComponent teamComponent = teamMembers[j];
                CharacterBody body = teamComponent.body;
                if (body)
                {
                    CharacterMaster master = teamComponent.body.master;
                    if (master)
                    {
                        int itemCount = master.inventory.GetItemCount(RoR2Content.Items.WardOnLevel);
                        if (itemCount > 0)
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(warbannerObject, body.transform.position, Quaternion.identity);
                            gameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                            gameObject.GetComponent<BuffWard>().Networkradius = 8f + 8f * (float)itemCount;
                            NetworkServer.Spawn(gameObject);
                        }
                    }
                }
            }
        }
    }
}
