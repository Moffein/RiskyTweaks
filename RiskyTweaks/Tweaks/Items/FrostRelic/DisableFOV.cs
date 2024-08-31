using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Items.FrostRelic
{
    public class DisableFOV : TweakBase<DisableFOV>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Client-Side) Frost Relic - Disable FOV";

        public override string ConfigDescriptionString => "Disables FOV modifier from this item.";

        protected override void ApplyChanges()
        {
            On.RoR2.IcicleAuraController.OnIciclesActivated += IcicleAuraController_OnIciclesActivated;
        }

        private void IcicleAuraController_OnIciclesActivated(On.RoR2.IcicleAuraController.orig_OnIciclesActivated orig, RoR2.IcicleAuraController self)
        {
            orig(self);

            CameraTargetParams.AimRequest aimRequest = self.aimRequest;
            if (aimRequest == null)
            {
                return;
            }
            aimRequest.Dispose();
        }
    }
}
