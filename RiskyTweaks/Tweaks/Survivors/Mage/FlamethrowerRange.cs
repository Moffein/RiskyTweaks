using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Mage
{
    public class FlamethrowerRange : TweakBase<FlamethrowerRange>
    {
        public override string ConfigCategoryString => "Survivors - Artificer";

        public override string ConfigOptionName => "(Client-Side) Flamethrower - Increase Range";

        public override string ConfigDescriptionString => "Increases Artificer's flamethrower range from 20m to 30m to match CHEF.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Mage/EntityStates.Mage.Weapon.Flamethrower.asset", "maxDistance", "30");  //20 vanilla, 30 is CHEF
        }
    }
}
