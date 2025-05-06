using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2.Projectile;
using MonoMod.Cil;

namespace RiskyTweaks.Tweaks.Survivors.CHEF
{
    public class SnappierCleaver : TweakBase<SnappierCleaver>
    {
        public override string ConfigCategoryString => "Survivors - CHEF";

        public override string ConfigOptionName => "(Server-Side) Snappier Dice";

        public override string ConfigDescriptionString => "Dice has a shorter minimum delay before returning.";

        protected override void ApplyChanges()
        {
            GameObject projectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Chef/ChefCleaver.prefab").WaitForCompletion();
            CleaverProjectile cleave = projectilePrefab.GetComponent<CleaverProjectile>();
            //Debug.Log("RiskyTweaks: Cleaver Charge: " + cleave.holdChargeTime);   //0.33
            //Debug.Log("RiskyTweaks: Cleaver Transition: " + cleave.transitionDuration);   //0.055
            //Debug.Log("RiskyTweaks: Cleaver min travel time: " + cleave.minTravelTime);  //ONE WHOLE SECOND
            cleave.minTravelTime = 0.2f;
        }
    }
}
