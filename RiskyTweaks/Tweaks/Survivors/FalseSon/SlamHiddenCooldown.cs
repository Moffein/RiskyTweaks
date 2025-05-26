using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.FalseSon
{
    public class SlamHiddenCooldown : TweakBase<SlamHiddenCooldown>
    {
        public override string ConfigCategoryString => "Survivors - False Son";

        public override string ConfigOptionName => "(Client-Side) Club of the Forsaken - Remove Slam Cooldown";

        public override string ConfigDescriptionString => "Removes hidden non-scaling cooldown from Club of the Forsaken's slam.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/DLC2/FalseSon/EntityStates.FalseSon.ClubSwing.asset", "secondaryAltCooldownDuration", "0");
        }
    }
}
