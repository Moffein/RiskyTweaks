using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies.Parent
{
    public class DirectorCost : TweakBase<DirectorCost>
    {
        public override string ConfigCategoryString => "Enemies - Parent";

        public override string ConfigOptionName => "(Server-Side) Lower Director Cost";

        public override string ConfigDescriptionString => "Lowers director cost to be in-line with the nerfed HP from the Anniversary update.";

        protected override void ApplyChanges()
        {
            CharacterSpawnCard csc = LegacyResourcesAPI.Load<CharacterSpawnCard>("spawncards/characterspawncards/cscparent");
            csc.directorCreditCost = 75;    //65 to be proportional to parent, 75 for Elder Lemurian
        }
    }
}
