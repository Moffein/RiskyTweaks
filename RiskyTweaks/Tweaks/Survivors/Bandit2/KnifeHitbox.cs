using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class KnifeHitbox : TweakBase<KnifeHitbox>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Knife Hitbox";

        public override string ConfigDescriptionString => "Increases knife hitbox size.";

        protected override void ApplyChanges()
        {
            CharacterBody cb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2Body.prefab").WaitForCompletion().GetComponent<CharacterBody>();
            HitBoxGroup hbg = cb.GetComponentInChildren<HitBoxGroup>();
            if (hbg.groupName == "SlashBlade")
            {
                //10.35, 4.25, 5.73
                Transform hitboxTransform = hbg.hitBoxes[0].transform;
                hitboxTransform.localScale = new Vector3(10.35f, 6f, 7.5f);
                hitboxTransform.localPosition += new Vector3(0f, 0f, 1f);
            }
        }
    }
}
