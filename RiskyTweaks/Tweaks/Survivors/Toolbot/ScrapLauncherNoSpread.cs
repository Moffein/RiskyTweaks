using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Toolbot
{
    public class ScrapLauncherNoSpread : TweakBase<ScrapLauncherNoSpread>
    {
        public override string ConfigCategoryString => "Survivors - MUL-T";

        public override string ConfigOptionName => "(Client-Side) Scrap Launcher - Remove Spread";

        public override string ConfigDescriptionString => "Removes random spread.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Toolbot/EntityStates.Toolbot.FireGrenadeLauncher.asset", "maxSpread", "0");
        }
    }
}
