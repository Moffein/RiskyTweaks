using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.VoidSurvivor
{
    public class FasterCorruptTransition : TweakBase<FasterCorruptTransition>
    {
        public override string ConfigCategoryString => "Survivors - Void Fiend";

        public override string ConfigOptionName => "(Client-Side) Corruption - Faster Animation";

        public override string ConfigDescriptionString => "Corruption transition animation is faster.";

        protected override void ApplyChanges()
        {
            On.EntityStates.VoidSurvivor.CorruptionTransitionBase.OnEnter += CorruptionTransitionBase_OnEnter;
        }

        private void CorruptionTransitionBase_OnEnter(On.EntityStates.VoidSurvivor.CorruptionTransitionBase.orig_OnEnter orig, EntityStates.VoidSurvivor.CorruptionTransitionBase self)
        {
            if (self.duration > 0f) //Exiting has 0 duration by default
            {
                self.duration = 0.5f;  //Default enter duration is 1f
            }
            orig(self);
        }
    }
}
