using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Railgunner
{
    public class PolarFieldBuff : TweakBase<PolarFieldBuff>
    {
        public override string ConfigCategoryString => "Survivors - Railgunner";

        public override string ConfigOptionName => "(Server-Side) Polar Field Damage Reduction";

        public override string ConfigDescriptionString => "Projectiles slowed by Polar Field deal less damage.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.Projectile.SlowDownProjectiles.OnTriggerEnter += SlowDownProjectiles_OnTriggerEnter;
            On.RoR2.Projectile.SlowDownProjectiles.OnTriggerExit += SlowDownProjectiles_OnTriggerExit;
        }

        private const float reductionFactor = 3f;

        private void SlowDownProjectiles_OnTriggerEnter(On.RoR2.Projectile.SlowDownProjectiles.orig_OnTriggerEnter orig, RoR2.Projectile.SlowDownProjectiles self, UnityEngine.Collider other)
        {
            {
                if (self.gameObject.name == "RailgunnerMineAltDetonated(Clone)")
                {
                    ProjectileDamage pd = other.GetComponent<ProjectileDamage>();
                    if (pd)
                    {
                        pd.damage /= reductionFactor;
                    }
                }
                orig(self, other);
            }
        }


        private void SlowDownProjectiles_OnTriggerExit(On.RoR2.Projectile.SlowDownProjectiles.orig_OnTriggerExit orig, RoR2.Projectile.SlowDownProjectiles self, UnityEngine.Collider other)
        {
            if (self.gameObject.name == "RailgunnerMineAltDetonated(Clone)")
            {
                ProjectileDamage pd = other.GetComponent<ProjectileDamage>();
                if (pd && other.GetComponent<ProjectileSimple>())
                {
                    pd.damage *= reductionFactor;
                }
            }
            orig(self, other);
        }
    }
}
