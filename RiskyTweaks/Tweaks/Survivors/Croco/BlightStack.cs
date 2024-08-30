using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Croco
{
    public class BlightStack : TweakBase<BlightStack>
    {
        public override string ConfigCategoryString => "Survivors - Acrid";

        public override string ConfigOptionName => "(Server-Side) Blight Duration Reset";

        public override string ConfigDescriptionString => "Blight duration resets when new stacks are added.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex)
        {
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
            if (dotIndex == DotController.DotIndex.Blight)
            {
                for (int i = 0; i < self.dotStackList.Count; i++)
                {
                    if (self.dotStackList[i].dotIndex == DotController.DotIndex.Blight)
                    {
                        self.dotStackList[i].timer = Mathf.Max(self.dotStackList[i].timer, duration);
                    }
                }
            }
        }
    }
}
