using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Stages.VoidStage
{
    public class NerfFog : TweakBase<NerfFog>
    {
        public override string ConfigCategoryString => "Stages - Void Locus";

        public override string ConfigOptionName => "(Server-Side) Nerf Fog";

        public override string ConfigDescriptionString => "Nerfs fog damage settings to be consistent with Void Fields.";

        protected override void ApplyChanges()
        {
            On.RoR2.VoidStageMissionController.RequestFog += VoidStageMissionController_RequestFog;
        }

        private RoR2.VoidStageMissionController.FogRequest VoidStageMissionController_RequestFog(On.RoR2.VoidStageMissionController.orig_RequestFog orig, RoR2.VoidStageMissionController self, RoR2.IZone zone)
        {
            if (self.fogDamageController)
            {
                self.fogDamageController.healthFractionPerSecond = 0.025f;//0.05
                self.fogDamageController.healthFractionRampCoefficientPerSecond = 0f;//0.1
            }
            return orig(self, zone);
        }
    }
}
