using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.Projectile;
using MonoMod.Cil;

namespace RiskyTweaks.Tweaks.Survivors.Seeker
{
    public class MeditateBuff : TweakBase<MeditateBuff>
    {
        public override string ConfigCategoryString => "Survivors - Seeker";

        public override string ConfigOptionName => "(Server-Side) Meditate Buff";

        public override string ConfigDescriptionString => "Meditate stuns and cleanses projectiles.";

        protected override void ApplyChanges()
        {
            On.RoR2.SeekerController.CmdTriggerHealPulse += SeekerController_CmdTriggerHealPulse;
            IL.EntityStates.Seeker.Meditate.Update += Meditate_Update;
        }

        private void Meditate_Update(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(x => x.MatchCallvirt<BlastAttack>("Fire")))
            {
                c.EmitDelegate<Func<BlastAttack, BlastAttack>>(blast =>
                {
                    blast.damageType.damageType |= DamageType.Stun1s;
                    return blast;
                });
            }
            else
            {
                Debug.LogError("RiskyTweaks: Meditate Stun IL Hook failed.");
            }
        }

        private void SeekerController_CmdTriggerHealPulse(On.RoR2.SeekerController.orig_CmdTriggerHealPulse orig, SeekerController self, float value, Vector3 corePosition, float blastRadius, float fxScale)
        {
            orig(self, value, corePosition, blastRadius, fxScale);

            if (!NetworkServer.active) return;
            List<ProjectileController> instancesList = InstanceTracker.GetInstancesList<ProjectileController>();
            List<GameObject> toDestroy = new List<GameObject>();
            foreach (ProjectileController pc in instancesList)
            {
                TeamIndex friendlyTeam = self.characterBody.teamComponent ? self.characterBody.teamComponent.teamIndex : TeamIndex.None;
                if (pc.cannotBeDeleted || pc.teamFilter.teamIndex == friendlyTeam || (pc.transform.position - self.transform.position).sqrMagnitude >= blastRadius * blastRadius) continue;
                toDestroy.Add(pc.gameObject);
            }

            GameObject[] toDestroy2 = toDestroy.ToArray();
            for (int i = 0; i < toDestroy2.Length; i++)
            {
                EffectManager.SimpleEffect(cleanseEffect, toDestroy2[i].transform.position, toDestroy2[i].transform.rotation, true);
                UnityEngine.Object.Destroy(toDestroy2[i]);
            }
        }
        private static GameObject cleanseEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Seeker/SpiritPunchMuzzleFlashVFX.prefab").WaitForCompletion();
    }
}
