using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.VoidSurvivor
{
    public class UtilityMoveSpeedScaling : TweakBase<UtilityMoveSpeedScaling>
    {
        public override string ConfigCategoryString => "Survivors - Void Fiend";

        public override string ConfigOptionName => "(Client-Side) Uncorrupted Trespass - Disable Move Speed Scaling";

        public override string ConfigDescriptionString => "Disables move speed scaling on Uncorrupted Trespass.";

        public override bool StopLoadOnConfigDisable => false;

        protected override void ApplyChanges()
        {
            On.EntityStates.VoidSurvivor.VoidBlinkBase.GetVelocity += VoidBlinkBase_GetVelocity;
        }

        private UnityEngine.Vector3 VoidBlinkBase_GetVelocity(On.EntityStates.VoidSurvivor.VoidBlinkBase.orig_GetVelocity orig, EntityStates.VoidSurvivor.VoidBlinkBase self)
        {
            if (Enabled.Value && self.characterBody && !self.characterBody.HasBuff(DLC1Content.Buffs.VoidSurvivorCorruptMode))
            {
                self.moveSpeedStat = 10.15f;//7f * 1.45f
            }
            return orig(self);
        }
    }
}
