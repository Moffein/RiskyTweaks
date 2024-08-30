using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Artifacts
{
    public class VengeancePercentHeal : TweakBase<VengeancePercentHeal>
    {
        public override string ConfigCategoryString => "Artifacts - Vengeance";

        public override string ConfigOptionName => "(Server-Side) Vengeance Percent Heal Nerf";

        public override string ConfigDescriptionString => "Vengeance Umbras receive reduced healing proportional to their HP bonus.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.HealthComponent.HealFraction += HealthComponent_HealFraction;
        }

        private float HealthComponent_HealFraction(On.RoR2.HealthComponent.orig_HealFraction orig, RoR2.HealthComponent self, float fraction, RoR2.ProcChainMask procChainMask)
        {
            if (self.itemCounts.invadingDoppelganger > 0)
            {
                fraction *= 0.1f;
            }
            return orig(self, fraction, procChainMask);
        }
    }
}
