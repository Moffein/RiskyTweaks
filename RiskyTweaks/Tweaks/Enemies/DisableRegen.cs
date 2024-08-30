using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class DisableRegen : TweakBase<DisableRegen>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) Disable Regen";

        public override string ConfigDescriptionString => "Removes passive health regen from the few enemies that have it.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            string[] paths =
            {
                "RoR2/Base/LunarGolem/LunarGolemBody.prefab",
                "RoR2/Base/Imp/ImpBody.prefab",
                "RoR2/Base/MiniMushroom/MiniMushroomBody.prefab",
                "RoR2/Base/HermitCrab/HermitCrabBody.prefab"
            };

            foreach (string str in paths)
            {
                RemoveRegen(str);
            }
        }

        private void RemoveRegen(string addressablePath)
        {
            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>(addressablePath).WaitForCompletion();
            if (!gameObject) return;
            CharacterBody body = gameObject.GetComponent<CharacterBody>();
            if (!body) return;

            body.baseRegen = 0f;
            body.levelRegen = 0f;
        }
    }
}
