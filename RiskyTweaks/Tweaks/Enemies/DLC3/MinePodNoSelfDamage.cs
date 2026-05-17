using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies.DLC3
{
    public class MinePodNoSelfDamage : TweakBase<MinePodNoSelfDamage>
    {
        public override string ConfigCategoryString => "Enemies - DLC3";

        public override string ConfigOptionName => "Solus Mines inherit killer team.";

        public override string ConfigDescriptionString => "Solus Mines inherit killer team to prevent self damage.";

        protected override void ApplyChanges()
        {
            On.EntityStates.SolusMine.EarlyDetonate.GetTeamIndex += EarlyDetonate_GetTeamIndex;
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            Addressables.LoadAssetAsync<GameObject>("RoR2/DLC3/SolusMine/SolusMineBody.prefab").WaitForCompletion().AddComponent<StoreKillerTeam>();
        }

        private RoR2.TeamIndex EarlyDetonate_GetTeamIndex(On.EntityStates.SolusMine.EarlyDetonate.orig_GetTeamIndex orig, EntityStates.SolusMine.EarlyDetonate self)
        {
            var killerTeam = self.gameObject.GetComponent<StoreKillerTeam>();
            if (killerTeam && killerTeam.teamIndex != TeamIndex.None)
            {
                return killerTeam.teamIndex;
            }

            return orig(self);
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if (damageReport.victimBodyIndex == DLC3Content.BodyPrefabs.SolusMineBody.bodyIndex && damageReport.victim && damageReport.attackerTeamIndex != TeamIndex.None)
            {
                var killerStorage = damageReport.victim.GetComponent<StoreKillerTeam>();
                if (killerStorage)
                {
                    killerStorage.teamIndex = damageReport.attackerTeamIndex;
                }
            }

            orig(self, damageReport);
        }

        private class StoreKillerTeam : MonoBehaviour
        {
            public TeamIndex teamIndex = TeamIndex.None;
        }
    }
}
