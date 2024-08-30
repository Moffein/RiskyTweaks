using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Croco
{
    public class BiggerHitbox : TweakBase<BiggerHitbox>
    {
        public override string ConfigCategoryString => "Survivors - Acrid";

        public override string ConfigOptionName => "(Client-Side) Bigger Melee Hitbox";

        public override string ConfigDescriptionString => "Increases melee hitbox size.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            CharacterBody cb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Croco/CrocoBody.prefab").WaitForCompletion().GetComponent<CharacterBody>();
            HitBoxGroup hbg = cb.GetComponentInChildren<HitBoxGroup>();
            if (hbg.groupName == "Slash")
            {
                Transform hitboxTransform = hbg.hitBoxes[0].transform;
                //Default: (34.8, 27.0, 34.4)
                hitboxTransform.localScale = new Vector3(40f, 40f, 45f);  //z is up/down

                //Defualt: (0.0, 13.0, 17.8)
                hitboxTransform.localPosition = new Vector3(0f, 11f, 15f);    //y is up/down
            }
        }
    }
}
