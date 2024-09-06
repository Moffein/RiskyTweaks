using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Mage
{
    public class FlamethrowerGuaranteedBurn : TweakBase<FlamethrowerGuaranteedBurn>
    {
        public override string ConfigCategoryString => "Survivors - Artificer";

        public override string ConfigOptionName => "(Client-Side) Flamethrower - Guaranteed Burn";

        public override string ConfigDescriptionString => "Increases Flamethrower's burn chance to 100% so that it's as strong as it was before SotV's burn nerfs.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Mage/EntityStates.Mage.Weapon.Flamethrower.asset", "ignitePercentChance", "100");
        }
    }
}
