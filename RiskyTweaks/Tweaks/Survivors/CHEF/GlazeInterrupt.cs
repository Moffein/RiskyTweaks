using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.CHEF
{
    public class GlazeInterrupt : TweakBase<GlazeInterrupt>
    {
        public override string ConfigCategoryString => "Survivors - CHEF";

        public override string ConfigOptionName => "(Client-Side) Glaze InterruptPriority";

        public override string ConfigDescriptionString => "Glaze is no longer interrupted by Sear.";

        protected override void ApplyChanges()
        {
            On.EntityStates.Chef.Glaze.GetMinimumInterruptPriority += Glaze_GetMinimumInterruptPriority;
        }

        private EntityStates.InterruptPriority Glaze_GetMinimumInterruptPriority(On.EntityStates.Chef.Glaze.orig_GetMinimumInterruptPriority orig, EntityStates.Chef.Glaze self)
        {
            return (self.grenadeCount < EntityStates.Chef.Glaze.grenadeCountMax) ? EntityStates.InterruptPriority.Pain : EntityStates.InterruptPriority.PrioritySkill;
        }
    }
}
