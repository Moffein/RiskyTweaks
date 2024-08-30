using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Artifacts
{
    public class VengeanceFallDamage : TweakBase<VengeanceFallDamage>
    {
        public override string ConfigCategoryString => "Artifacts - Vengeance";

        public override string ConfigOptionName => "(Server-Side) No Fall Damage";

        public override string ConfigDescriptionString => "Vengeance Umbras are immune to fall damage.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
        }

        private void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if (NetworkServer.active && (damageInfo.damageType & DamageType.FallDamage) != 0)
            {
                if (self.itemCounts.invadingDoppelganger > 0)
                {
                    damageInfo.damage = 0f;
                    damageInfo.rejected = true;
                }
            }
            orig(self, damageInfo);
        }
    }
}
