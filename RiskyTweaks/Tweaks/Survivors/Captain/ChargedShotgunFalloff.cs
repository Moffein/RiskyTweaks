using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Captain
{
    public class ChargedShotgunFalloff : TweakBase<ChargedShotgunFalloff>
    {
        public override string ConfigCategoryString => "Survivors - Captain";

        public override string ConfigOptionName => "(Client-Side) Charged Shotgun Falloff";

        public override string ConfigDescriptionString => "Vulcan Shotgun has no falloff when fully charged";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += FireCaptainShotgun_ModifyBullet;
        }

        private void FireCaptainShotgun_ModifyBullet(On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, RoR2.BulletAttack bulletAttack)
        {
            orig(self, bulletAttack);
            if (self.fireSoundString == "FireCaptainShotgun.tightSoundString")
            {
                bulletAttack.falloffModel = RoR2.BulletAttack.FalloffModel.None;
            }
        }
    }
}
