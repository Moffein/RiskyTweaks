using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.FalseSon
{
    public class LaserProcCoefficient : TweakBase<LaserProcCoefficient>
    {
        public override string ConfigCategoryString => "Survivors - False Son";

        public override string ConfigOptionName => "(Client-Side) Laser of the Father - Increase Proc Coefficient";

        public override string ConfigDescriptionString => "Increases Laser of the Father proc coefficient to 1.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/DLC2/FalseSon/EntityStates.FalseSon.LaserFatherCharged.asset", "procCoefficientPerTick", "1");
        }
    }
}
