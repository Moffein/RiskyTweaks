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
