using EntityStates;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class CloakHoldButton : TweakBase<CloakHoldButton>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Smokebomb Auto Trigger on Hold";

        public override string ConfigDescriptionString => "Smokebomb automatically triggers as soon as it is available if the button is held.";

        private const float minCloakDuration = 0.3f;

        protected override void ApplyChanges()
        {
            Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Bandit2/ThrowSmokebomb.asset").WaitForCompletion().mustKeyPress = false;
            On.EntityStates.Bandit2.ThrowSmokebomb.GetMinimumInterruptPriority += ThrowSmokebomb_GetMinimumInterruptPriority;
            On.EntityStates.Bandit2.StealthMode.GetMinimumInterruptPriority += StealthMode_GetMinimumInterruptPriority;
        }

        private InterruptPriority StealthMode_GetMinimumInterruptPriority(On.EntityStates.Bandit2.StealthMode.orig_GetMinimumInterruptPriority orig, EntityStates.Bandit2.StealthMode self)
        {
            return self.fixedAge > minCloakDuration ? InterruptPriority.Skill : InterruptPriority.Frozen;
        }

        private EntityStates.InterruptPriority ThrowSmokebomb_GetMinimumInterruptPriority(On.EntityStates.Bandit2.ThrowSmokebomb.orig_GetMinimumInterruptPriority orig, EntityStates.Bandit2.ThrowSmokebomb self)
        {
            return self.fixedAge > minCloakDuration ? InterruptPriority.PrioritySkill : InterruptPriority.Pain;
        }
    }
}
