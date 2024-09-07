using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.VoidStage
{
    public class FasterHoldout : TweakBase<FasterHoldout>
    {
        public override string ConfigCategoryString => "Stages - Void Locus";

        public override string ConfigOptionName => "(Server-Side) Faster Holdout";

        public override string ConfigDescriptionString => "Reduce time it takes to charge Void Signals.";

        protected override void ApplyChanges()
        {
            if (ModCompat.TeleExpansionCompat.pluginLoaded)
            {
                Debug.LogWarning("RiskyTweaks: Skipped Void Locus FasterHoldout because TeleExpansion is loaded.");
                return;
            }

            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion();
            HoldoutZoneController hzc = gameObject.GetComponent<HoldoutZoneController>();
            if (!hzc) return;
            hzc.baseChargeDuration = 40f;   //60f vanilla
        }
    }
}
