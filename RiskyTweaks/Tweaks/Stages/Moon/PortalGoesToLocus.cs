using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Stages.Moon
{
    public class PortalGoesToLocus : TweakBase<PortalGoesToLocus>
    {
        public override string ConfigCategoryString => "Stages - Commencement";

        public override string ConfigOptionName => "(Server-Side) Frog leads to Void Locus";

        public override string ConfigDescriptionString => "Petting the frog leads to Void Locus instead of going directly to Voidling.";

        protected override void ApplyChanges()
        {
            GameObject frog = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/moon/FrogInteractable.prefab").WaitForCompletion();
            var portalSpawner = frog.GetComponent<PortalSpawner>();
            portalSpawner.portalSpawnCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/PortalVoid/iscVoidPortal.asset").WaitForCompletion();
            portalSpawner.spawnReferenceLocation.localPosition += Vector3.down * 5;
        }
    }
}
