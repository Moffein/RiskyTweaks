using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.VoidRaid
{
    public class NerfFog : TweakBase<NerfFog>
    {
        public override string ConfigCategoryString => "Stages - Planetarium";

        public override string ConfigOptionName => "(Server-Side) Nerf Fog";

        public override string ConfigDescriptionString => "Nerfs fog damage settings to be consistent with Void Fields.";

        private SceneDef voidRaidScene = Addressables.LoadAssetAsync<SceneDef>("RoR2/DLC1/voidraid/voidraid.asset").WaitForCompletion();

        protected override void ApplyChanges()
        {
            On.RoR2.FogDamageController.Start += FogDamageController_Start;
        }

        //Bruteforce way of doing this
        private void FogDamageController_Start(On.RoR2.FogDamageController.orig_Start orig, FogDamageController self)
        {
            orig(self);
            if (SceneCatalog.GetSceneDefForCurrentScene() == voidRaidScene)
            {
                //Debug.Log("HP% per second: " + self.healthFractionPerSecond);   //0.01
                //Debug.Log("HP% Ramping per second: " + self.healthFractionRampCoefficientPerSecond + "\n");    //0.1

                self.healthFractionPerSecond = 0.025f;
                self.healthFractionRampCoefficientPerSecond = 0f;
            }
        }
    }
}
