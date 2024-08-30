using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies.LunarWisp
{
    public class IncreaseDirectorCost : TweakBase<IncreaseDirectorCost>
    {
        public override string ConfigCategoryString => "Enemies - Lunar Wisp";

        public override string ConfigOptionName => "(Server-Side) Increase Director Cost";

        public override string ConfigDescriptionString => "Increases director cost to be more in-line with other flying enemies.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            CharacterSpawnCard csc = Addressables.LoadAssetAsync<CharacterSpawnCard>("RoR2/Base/LunarWisp/cscLunarWisp.asset").WaitForCompletion();
            csc.directorCreditCost = 700; //550 vanilla, 350 for lunar golem
        }
    }
}
