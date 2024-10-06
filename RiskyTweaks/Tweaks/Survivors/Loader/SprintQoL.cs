using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Loader
{
    public class SprintQoL : TweakBase<SprintQoL>
    {
        public override string ConfigCategoryString => "Survivors - Loader";

        public override string ConfigOptionName => "(Client-Side) Sprint QoL";

        public override string ConfigDescriptionString => "Non-primary skills don't cancel sprinting.";

        public override bool StopLoadOnConfigDisable => true;

        //TODO: Risk of Options
        protected override void ApplyChanges()
        {
            SkillDef defaultGrapple = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/FireHook.asset").WaitForCompletion();
            SkillDef altGrapple = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/FireYankHook.asset").WaitForCompletion();

            SkillDef defaultShift = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/ChargeFist.asset").WaitForCompletion();
            SkillDef altShift = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/ChargeZapFist.asset").WaitForCompletion();

            defaultGrapple.cancelSprintingOnActivation = false;
            altGrapple.cancelSprintingOnActivation = false;

            defaultShift.cancelSprintingOnActivation = false;
            altShift.cancelSprintingOnActivation = false;


            SkillDef slamDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/GroundSlam.asset").WaitForCompletion();
            slamDef.cancelSprintingOnActivation = false;


            SkillDef pylonDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Loader/ThrowPylon.asset").WaitForCompletion();
            pylonDef.cancelSprintingOnActivation = false;
        }
    }
}
