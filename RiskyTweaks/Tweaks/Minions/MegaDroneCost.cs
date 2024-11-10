using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;

namespace RiskyTweaks.Tweaks.Minions
{
    public class MegaDroneCost : TweakBase<MegaDroneCost>
    {
        public override string ConfigCategoryString => "Minions";

        public override string ConfigOptionName => "(Server-Side) TC-280 - Cost";

        public override string ConfigDescriptionString => "Reduces the purchase price of this drone.";

        protected override void ApplyChanges()
        {
            base.ApplyChanges();

            GameObject megaDroneBrokenObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Drones/MegaDroneBroken.prefab").WaitForCompletion();
            PurchaseInteraction pi = megaDroneBrokenObject.GetComponent<PurchaseInteraction>();
            pi.cost = 300;  //Vanilla is 350
        }
    }
}
