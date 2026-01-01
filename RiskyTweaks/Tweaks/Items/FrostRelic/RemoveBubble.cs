using RoR2;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Items.FrostRelic
{
    public class RemoveBubble : TweakBase<RemoveBubble>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Client-Side) Frost Relic - Remove Bubble";

        public override string ConfigDescriptionString => "Removes bubble VFX from this item.";

        protected override void ApplyChanges()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Gorakh.ItemQualities"))
            {
                Debug.LogWarning("RiskyTweaks: Skipping FrostRelic RemoveBubble because Quality is installed, to prevent incompats.");
                return;
            }

            GameObject indicator = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/IcicleAura");
            ParticleSystemRenderer[] pr = indicator.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (ParticleSystemRenderer p in pr)
            {
                if (p.name == "Area")
                {
                    Object.Destroy(p);
                }
            }
        }
    }
}
