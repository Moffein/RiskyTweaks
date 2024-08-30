using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Items
{
    public class VisionsAttackSpeed : TweakBase<VisionsAttackSpeed>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Client-Side) Visions of Heresy - Attack Speed Scaling";

        public override string ConfigDescriptionString => "Visions of Heresy cooldown scales with Attack Speed.";

        protected override void ApplyChanges()
        {
            LunarPrimaryReplacementSkill visionsDef = Addressables.LoadAssetAsync<LunarPrimaryReplacementSkill>("RoR2/Base/LunarSkillReplacements/LunarPrimaryReplacement.asset").WaitForCompletion();
            visionsDef.attackSpeedBuffsRestockSpeed = true;
            visionsDef.attackSpeedBuffsRestockSpeed_Multiplier = 1f;
        }
    }
}
