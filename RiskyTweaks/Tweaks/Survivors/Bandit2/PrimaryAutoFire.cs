using EntityStates;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class PrimaryAutoFire : TweakBase<PrimaryAutoFire>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Primary Autofire";

        public override string ConfigDescriptionString => "Primaries automatically fire when holding down the button.";

        protected override void ApplyChanges()
        {
            ReloadSkillDef shotgun = Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/FireShotgun2.asset").WaitForCompletion();
            shotgun.mustKeyPress = false;
            shotgun.interruptPriority = EntityStates.InterruptPriority.Skill;

            ReloadSkillDef rifle = Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/Bandit2Blast.asset").WaitForCompletion();
            rifle.mustKeyPress = false;
            rifle.interruptPriority = EntityStates.InterruptPriority.Skill;


            On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.GetMinimumInterruptPriority += Bandit2FirePrimaryBase_GetMinimumInterruptPriority;
            On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.OnEnter += Bandit2FirePrimaryBase_OnEnter;
        }

        private void Bandit2FirePrimaryBase_OnEnter(On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.orig_OnEnter orig, EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase self)
        {
            /*if (fireMode == BanditFireMode.Tap)
            {
                self.minimumBaseDuration = autoFireDuration;
            }
            else
            {
                self.minimumBaseDuration = burstFireDuration;
            }*/
            self.minimumBaseDuration = 0.3f;
            orig(self);
        }

        private InterruptPriority Bandit2FirePrimaryBase_GetMinimumInterruptPriority(On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.orig_GetMinimumInterruptPriority orig, EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase self)
        {
            if (self.fixedAge <= self.minimumDuration && self.inputBank.skill1.wasDown)
            {
                return InterruptPriority.PrioritySkill;
            }
            return InterruptPriority.Any;
        }
    }
}
