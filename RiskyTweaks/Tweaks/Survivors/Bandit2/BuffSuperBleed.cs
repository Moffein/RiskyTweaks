using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class BuffSuperBleed : TweakBase<BuffSuperBleed>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Server-Side) Hemorrhage Ignores Armor";

        public override string ConfigDescriptionString => "Hemorrhage ignores positive armor.";

        protected override void ApplyChanges()
        {
            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
        }

        private void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if (damageInfo.dotIndex == RoR2.DotController.DotIndex.SuperBleed && ((damageInfo.damageType & DamageType.DoT) != 0) && ((damageInfo.damageType & DamageType.AOE) == 0))
            {
                float totalArmor = self.body.armor + self.adaptiveArmorValue;
                if (totalArmor > 0f)
                {
                    damageInfo.damage *= (100f + totalArmor) / 100f;
                }
            }
            orig(self, damageInfo);
        }
    }
}
