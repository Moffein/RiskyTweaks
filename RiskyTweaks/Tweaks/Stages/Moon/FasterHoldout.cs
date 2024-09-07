using UnityEngine;
using RoR2;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.Moon
{
    public class FasterHoldout : TweakBase<FasterHoldout>
    {
        public override string ConfigCategoryString => "Stages - Commencement";

        public override string ConfigOptionName => "(Server-Side) Faster Holdout";

        public override string ConfigDescriptionString => "Reduce time it takes to charge the Pillar of Mass and Soul.";

        protected override void ApplyChanges()
        {
            if (ModCompat.TeleExpansionCompat.pluginLoaded)
            {
                Debug.LogWarning("RiskyTweaks: Skipped Commencement FasterHoldout because TeleExpansion is loaded.");
                return;
            }

            ModifyHoldout(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/moon2/MoonBatteryMass.prefab").WaitForCompletion(), 40f);
            ModifyHoldout(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/moon2/MoonBatterySoul.prefab").WaitForCompletion(), 20f);
            //"RoR2/Base/moon2/MoonBatteryDesign.prefab" //Design is 30s, this is fine
            //"RoR2/Base/moon2/MoonBatteryBlood.prefab" //Blood is 10s, this is fine
        }

        private void ModifyHoldout(GameObject gameObject, float durationOverride)
        {
            if (!gameObject) return;

            HoldoutZoneController hzc = gameObject.GetComponent<HoldoutZoneController>();
            if (!hzc) return;

            hzc.baseChargeDuration = durationOverride;
        }
    }
}
