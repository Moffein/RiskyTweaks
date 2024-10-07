using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.Heretic"))
            {
                UnityEngine.Debug.LogWarning("RiskyTweaks: Skipping Visions Attack Speed because Heretic is loaded.");
                return;
            }
            LunarPrimaryReplacementSkill visionsDef = Addressables.LoadAssetAsync<LunarPrimaryReplacementSkill>("RoR2/Base/LunarSkillReplacements/LunarPrimaryReplacement.asset").WaitForCompletion();
            visionsDef.attackSpeedBuffsRestockSpeed = true;
            visionsDef.attackSpeedBuffsRestockSpeed_Multiplier = 1f;

            //LunarPrimaryReplacement overrides the attackSpeedBuffsRestockSpeed stat
            On.RoR2.Skills.LunarPrimaryReplacementSkill.GetRechargeInterval += LunarPrimaryReplacementSkill_GetRechargeInterval;
        }

        private float LunarPrimaryReplacementSkill_GetRechargeInterval(On.RoR2.Skills.LunarPrimaryReplacementSkill.orig_GetRechargeInterval orig, LunarPrimaryReplacementSkill self, RoR2.GenericSkill skillSlot)
        {
            float interval = orig(self, skillSlot);

            if (self.attackSpeedBuffsRestockSpeed && skillSlot)
            {
                float num = skillSlot.characterBody.attackSpeed - skillSlot.characterBody.baseAttackSpeed;
                num *= self.attackSpeedBuffsRestockSpeed_Multiplier;
                num += 1f;
                if (num < 0.5f)
                {
                    num = 0.5f;
                }
                interval /= num;
            }

            return interval;
        }
    }
}
