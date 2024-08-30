using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class SpecialSprint : TweakBase<SpecialSprint>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Special Disable Sprint Cancel";

        public override string ConfigDescriptionString => "Sprinting no longer cancels Revolver skills.";

        protected override void ApplyChanges()
        {
            Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Bandit2/SkullRevolver.asset").WaitForCompletion().canceledFromSprinting = false;
            Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Bandit2/ResetRevolver.asset").WaitForCompletion().canceledFromSprinting = false;
            IL.EntityStates.Bandit2.Weapon.BaseSidearmState.FixedUpdate += BaseSidearmState_FixedUpdate;
        }

        private void BaseSidearmState_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchCallvirt(typeof(CharacterBody), "get_isSprinting")))
            {
                c.EmitDelegate<Func<bool, bool>>(isSprinting => false);
            }
            else
            {
                Debug.LogError("RiskyTweaks: Bandit SpecialSprint IL Hook failed.");
            }
        }
    }
}
