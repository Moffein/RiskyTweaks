using UnityEngine;
using RoR2;
using EntityStates;
using MonoMod.RuntimeDetour;
using R2API.Utils;
using UnityEngine.AddressableAssets;
using RoR2.Skills;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class KnifeLunge : TweakBase<KnifeLunge>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Knife Lunge";

        public override string ConfigDescriptionString => "Lunge forwards when using Serrated Dagger.";


        private AnimationCurve knifeVelocity;

        protected override void ApplyChanges()
        {
            knifeVelocity = BuildSlashVelocityCurve();

            SkillDef knifeSkillDef = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Bandit2/SlashBlade.asset").WaitForCompletion();

            knifeSkillDef.canceledFromSprinting = false;

            On.EntityStates.Bandit2.Weapon.SlashBlade.OnEnter += SlashBlade_OnEnter;

            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Bandit2/EntityStates.Bandit2.Weapon.SlashBlade.asset", "ignoreAttackSpeed", "1");
            var getBandit2SlashBladeMinDuration = new Hook(typeof(EntityStates.Bandit2.Weapon.SlashBlade).GetMethodCached("get_minimumDuration"), typeof(KnifeLunge).GetMethodCached(nameof(GetBandit2SlashBladeMinDurationHook)));
        }

        private void SlashBlade_OnEnter(On.EntityStates.Bandit2.Weapon.SlashBlade.orig_OnEnter orig, EntityStates.Bandit2.Weapon.SlashBlade self)
        {
            orig(self);
            if (self.characterBody && self.characterBody.isSprinting)
            {
                self.ignoreAttackSpeed = true;
                self.forceForwardVelocity = true;
                self.forwardVelocityCurve = knifeVelocity;
            }
        }

        //Does this actually need to be static?
        private static float GetBandit2SlashBladeMinDurationHook(EntityStates.Bandit2.Weapon.SlashBlade self)
        {
            return 0.3f;
        }

        private AnimationCurve BuildSlashVelocityCurve()
        {
            Keyframe kf1 = new Keyframe(0f, 3f, -8.182907104492188f, -3.3333332538604738f, 0f, 0.058712735772132876f);
            kf1.weightedMode = WeightedMode.None;
            kf1.tangentMode = 65;

            Keyframe kf2 = new Keyframe(0.3f, 0f, -3.3333332538604738f, -3.3333332538604738f, 0.3333333432674408f, 0.3333333432674408f);    //Time should match up with SlashBlade min duration (hitbox length)
            kf2.weightedMode = WeightedMode.None;
            kf2.tangentMode = 34;

            Keyframe[] keyframes = new Keyframe[2];
            keyframes[0] = kf1;
            keyframes[1] = kf2;

            return new AnimationCurve
            {
                preWrapMode = WrapMode.ClampForever,
                postWrapMode = WrapMode.ClampForever,
                keys = keyframes
            };
        }
    }
}
