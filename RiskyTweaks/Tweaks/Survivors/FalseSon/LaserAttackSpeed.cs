using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.FalseSon
{
    public class LaserAttackSpeed : TweakBase<LaserAttackSpeed>
    {
        public override string ConfigCategoryString => "Survivors - False Son";

        public override string ConfigOptionName => "(Client-Side) Laser of the Father - Attack Speed Scaling";

        public override string ConfigDescriptionString => "Laser of the Father properly scales tickrate with attack speed.";

        protected override void ApplyChanges()
        {
            On.EntityStates.FalseSon.LaserFatherCharged.OnEnter += LaserFatherCharged_OnEnter;
        }

        private void LaserFatherCharged_OnEnter(On.EntityStates.FalseSon.LaserFatherCharged.orig_OnEnter orig, EntityStates.FalseSon.LaserFatherCharged self)
        {
            orig(self);
            self.fireFrequency = EntityStates.FalseSon.LaserFatherCharged.baseFireFrequency * self.attackSpeedStat;
        }
    }
}
