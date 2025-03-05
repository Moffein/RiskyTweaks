using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.Arena
{
    public class RemoveItemGain : TweakBase<RemoveItemGain>
    {
        public override string ConfigCategoryString => "Stages - Void Fields";

        public override string ConfigOptionName => "(Server-Side) Remove SotS Part 2 Item Gain";

        public override string ConfigDescriptionString => "Reverts the SotS Part 2 change that makes enemies gain items while you are outside the bubble.";

        private SceneDef arenaScene = Addressables.LoadAssetAsync<SceneDef>("RoR2/Base/arena/arena.asset").WaitForCompletion();

        protected override void ApplyChanges()
        {
            On.EntityStates.Missions.Arena.NullWard.Active.HandlePenalties += Active_HandlePenalties;
        }

        private void Active_HandlePenalties(On.EntityStates.Missions.Arena.NullWard.Active.orig_HandlePenalties orig, EntityStates.Missions.Arena.NullWard.Active self, ref float percentOfPlayersInRadius)
        {
            self.timeSincePreviousItemPunishment = 0f;
            orig(self, ref percentOfPlayersInRadius);
        }
    }
}
