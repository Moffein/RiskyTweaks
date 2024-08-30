using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;

namespace RiskyTweaks.Tweaks.Enemies.Brother
{
    public class NoFallDamage : TweakBase<NoFallDamage>
    {
        public override string ConfigCategoryString => "Enemies - Mithrix";

        public override string ConfigOptionName => "(Server-Side) No Fall Damage";

        public override string ConfigDescriptionString => "Prevents Mithrix from killing himself with fall damage.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            GameObject brotherObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherBody.prefab").WaitForCompletion();
            CharacterBody brotherBody = brotherObject.GetComponent<CharacterBody>();
            brotherBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            GameObject brotherHurtObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            CharacterBody brotherHurtBody = brotherHurtObject.GetComponent<CharacterBody>();
            brotherHurtBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
        }
    }
}
