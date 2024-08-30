using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;

namespace RiskyTweaks.Tweaks.Equipment
{
    public class CaffeinatorPressurePlate : TweakBase<CaffeinatorPressurePlate>
    {
        public override string ConfigCategoryString => "Equipment";

        public override string ConfigOptionName => "(Server-Side) Remote Caffeinator - Trigger Pressure Plates";

        public override string ConfigDescriptionString => "Rermote Caffeinator can trigger pressure plates on Abandoned Aqueduct.";
        protected override void ApplyChanges()
        {
            GameObject vendingPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();
            CapsuleCollider cl = vendingPrefab.AddComponent<CapsuleCollider>();
            vendingPrefab.layer = LayerIndex.fakeActor.intVal;
        }
    }
}
