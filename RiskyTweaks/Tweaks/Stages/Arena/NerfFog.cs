using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.Arena
{
    public class NerfFog : TweakBase<NerfFog>
    {
        public override string ConfigCategoryString => "Stages - Void Fields";

        public override string ConfigOptionName => "(Server-Side) Remove SotS Part 2 Ramping Fog Damage";

        public override string ConfigDescriptionString => "Reverts SotS Phase 2 teamwide fog damage penalties.";

        private SceneDef arenaScene = Addressables.LoadAssetAsync<SceneDef>("RoR2/Base/arena/arena.asset").WaitForCompletion();

        protected override void ApplyChanges()
        {
            On.RoR2.ArenaMissionController.UpdateFogDamageVoidFieldsDamageMultiplier += ArenaMissionController_UpdateFogDamageVoidFieldsDamageMultiplier;
        }

        private void ArenaMissionController_UpdateFogDamageVoidFieldsDamageMultiplier(On.RoR2.ArenaMissionController.orig_UpdateFogDamageVoidFieldsDamageMultiplier orig, ArenaMissionController self, int voidFieldsDamageMultiplier)
        {
            return;
        }
    }
}
