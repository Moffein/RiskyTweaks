using RoR2.Projectile;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Items
{
    public class ShurikenBand : TweakBase<ShurikenBand>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Shuriken - No Band Proc";

        public override string ConfigDescriptionString => "Shuriken doesn't trigger elemental bands.";

        //Can't IL hook because vanilla method doesn't take ProcChainMask as an argument.
        protected override void ApplyChanges()
        {
            On.RoR2.PrimarySkillShurikenBehavior.FireShuriken += PrimarySkillShurikenBehavior_FireShuriken;
        }

        private void PrimarySkillShurikenBehavior_FireShuriken(On.RoR2.PrimarySkillShurikenBehavior.orig_FireShuriken orig, PrimarySkillShurikenBehavior self)
        {
            Ray aimRay = self.GetAimRay();
            FireProjectileInfo fpi = new FireProjectileInfo
            {
                projectilePrefab = self.projectilePrefab,
                position = aimRay.origin,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction) * self.GetRandomRollPitch(),
                owner = self.gameObject,
                damage = self.body.damage * (3f + 1f * (float)self.stack),
                force = 0f,
                crit = Util.CheckRoll(self.body.crit, self.body.master),
                damageColorIndex = DamageColorIndex.Item,
                damageTypeOverride = null,
                speedOverride = -1f,
                procChainMask = default(ProcChainMask)
            };
            fpi.procChainMask.AddProc(ProcType.Rings);
            ProjectileManager.instance.FireProjectile(fpi);
        }
    }
}
