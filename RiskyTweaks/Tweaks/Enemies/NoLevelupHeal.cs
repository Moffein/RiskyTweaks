using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class NoLevelupHeal : TweakBase<NoLevelupHeal>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) No Levelup Heal";

        public override string ConfigDescriptionString => "Enemies dont regain HP on levelup.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            float oldLevel = self.level;
            float oldHP = self.healthComponent.health;
            float oldShield = self.healthComponent.shield;
            orig(self);
            if (self.level > oldLevel)
            {
                if (self.teamComponent.teamIndex != TeamIndex.Player && self.healthComponent.combinedHealthFraction < 1f)
                {
                    if (self.healthComponent.health > oldHP)
                    {
                        self.healthComponent.health = oldHP;
                    }

                    if (self.healthComponent.shield > oldShield)
                    {
                        self.healthComponent.shield = oldShield;
                    }
                }
            }
        }
    }
}
