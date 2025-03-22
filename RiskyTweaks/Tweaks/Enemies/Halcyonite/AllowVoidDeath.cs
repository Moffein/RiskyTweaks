using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies.Halcyonite
{
    public class AllowVoidDeath : TweakBase<AllowVoidDeath>
    {
        public override string ConfigCategoryString => "Enemies - Halcyonite";

        public override string ConfigOptionName => "(Server-Side) Remove Void Immunity";

        public override string ConfigDescriptionString => "Allow this enemy to die to Void Implosions.";

        protected override void ApplyChanges()
        {
            CharacterBody body = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Halcyonite/HalcyoniteBody.prefab").WaitForCompletion().GetComponent<CharacterBody>();
            body.bodyFlags &= ~CharacterBody.BodyFlags.ImmuneToVoidDeath;
        }
    }
}
