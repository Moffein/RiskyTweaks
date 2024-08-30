using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Merc
{
    public class M1AttackSpeed : TweakBase<M1AttackSpeed>
    {
        public override string ConfigCategoryString => "Survivors - Mercenary";

        public override string ConfigOptionName => "(Client-Side) Primary Attack Speed Tweak";

        public override string ConfigDescriptionString => "Third primary hit doesnt scale with attack speed to allow for Expose combos.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += GroundLight2_OnEnter;
        }

        private void GroundLight2_OnEnter(On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, EntityStates.Merc.Weapon.GroundLight2 self)
        {
            if (self.isComboFinisher)
            {
                self.ignoreAttackSpeed = true;
            }
            else
            {
                self.ignoreAttackSpeed = false;
            }
            orig(self);
        }
    }
}
