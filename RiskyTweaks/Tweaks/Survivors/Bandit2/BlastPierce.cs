using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class BlastPierce : TweakBase<BlastPierce>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Blast - Pierce Enemies";

        public override string ConfigDescriptionString => "Blast can pierce through multiple enemies.";

        protected override void ApplyChanges()
        {
            On.EntityStates.Bandit2.Weapon.Bandit2FireRifle.ModifyBullet += Bandit2FireRifle_ModifyBullet;
        }

        private void Bandit2FireRifle_ModifyBullet(On.EntityStates.Bandit2.Weapon.Bandit2FireRifle.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.Bandit2FireRifle self, RoR2.BulletAttack bulletAttack)
        {
            orig(self, bulletAttack);
            bulletAttack.stopperMask = LayerIndex.world.mask;
        }
    }
}
