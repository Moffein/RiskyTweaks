using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.FalseSon
{
    public class SlamProcCoefficient : TweakBase<SlamProcCoefficient>
    {
        public override string ConfigCategoryString => "Survivors - False Son";

        public override string ConfigOptionName => "(Client-Side) Club of the Forsaken - Reduce Proc Coefficient";

        public override string ConfigDescriptionString => "Reduces fully-charged Club of the Forsaken proc coefficient to 1.";

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/DLC2/FalseSon/EntityStates.FalseSon.ChargedClubSwing.asset", "unchargedBlastProcCoefficient", "1");
            SneedUtils.SetAddressableEntityStateField("RoR2/DLC2/FalseSon/EntityStates.FalseSon.ChargedClubSwing.asset", "blastProcCoefficient", "1");
            SneedUtils.SetAddressableEntityStateField("RoR2/DLC2/FalseSon/EntityStates.FalseSon.ClubGroundSlam.asset", "blastProcCoefficient", "1");
        }
    }
}
