using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Stages
{
    public class ArtifactWorldDropChance : TweakBase<ArtifactWorldDropChance>
    {
        public override string ConfigCategoryString => "Stages";

        public override string ConfigOptionName => "(Server-Side) Bulwarks Ambry - Anti Softlock";

        public override string ConfigDescriptionString => "Artifact Keys are guaranteed to drop after a certain amount of kills.";

        public override bool StopLoadOnConfigDisable => true;


        private static int killCount = 0;

        protected override void ApplyChanges()
        {//Reset killcount on stage start
            RoR2.Stage.onStageStartGlobal += ResetKillCount;

            On.RoR2.ArtifactTrialMissionController.CombatState.OnCharacterDeathGlobal += ArtifactKeyDrop;

            //Increase base dropchance
            On.RoR2.ArtifactTrialMissionController.OnStartServer += ArtifactTrialMissionController_OnStartServer;
        }

        private void ArtifactTrialMissionController_OnStartServer(On.RoR2.ArtifactTrialMissionController.orig_OnStartServer orig, RoR2.ArtifactTrialMissionController self)
        {
            orig(self);
            if (self.chanceForKeyDrop < 0.04f)
            {
                self.chanceForKeyDrop = 0.04f;
            }
        }

        private void ResetKillCount(Stage obj)
        {
            killCount = 0;
        }

        private void ArtifactKeyDrop(On.RoR2.ArtifactTrialMissionController.CombatState.orig_OnCharacterDeathGlobal orig, EntityStates.EntityState self, DamageReport damageReport)
        {
            ArtifactTrialMissionController.ArtifactTrialMissionControllerBaseState artifactState = self as ArtifactTrialMissionController.ArtifactTrialMissionControllerBaseState;
            bool willDrop = Util.CheckRoll(Util.GetExpAdjustedDropChancePercent(artifactState.missionController.chanceForKeyDrop * 100f, damageReport.victim.gameObject), 0f, null);
            int maxKills = Mathf.CeilToInt(1f / artifactState.missionController.chanceForKeyDrop);

            killCount++;
            if (killCount > maxKills)
            {
                willDrop = true;
            }

            if (willDrop)
            {
                killCount = 0;
                Debug.LogFormat("Creating artifact key pickup droplet.", Array.Empty<object>());
                PickupDropletController.CreatePickupDroplet(artifactState.missionController.GenerateDrop(), damageReport.victimBody.corePosition, Vector3.up * 20f);
            }
        }
    }
}
