using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class AllowFallDeath : TweakBase<AllowFallDeath>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) Lethal Fall Damage";

        public override string ConfigDescriptionString => "Allows monsters to die from fall damage and increases fall damage taken.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
        }

        private void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            bool isPlayer = self.body.isPlayerControlled || (self.body.teamComponent && self.body.teamComponent.teamIndex == TeamIndex.Player);
            if (!isPlayer)
            {
                if ((damageInfo.damageType & DamageType.FallDamage & DamageType.NonLethal) != 0)
                {
                    damageInfo.damageType &= ~DamageType.NonLethal;
                    damageInfo.damageType |= DamageType.BypassOneShotProtection;

                    damageInfo.damage *= 1.5f;
                }
            }
            orig(self, damageInfo);
        }
    }
}
