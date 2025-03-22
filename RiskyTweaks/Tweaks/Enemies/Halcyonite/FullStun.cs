using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Linq;

namespace RiskyTweaks.Tweaks.Enemies.Halcyonite
{
    public class FullStun : TweakBase<FullStun>
    {
        public override string ConfigCategoryString => "Enemies - Halcyonite";

        public override string ConfigOptionName => "(Server-Side) Allow Attack Interrupt";

        public override string ConfigDescriptionString => "Removes stun and freeze immunity from certain states.";

        protected override void ApplyChanges()
        {
            GameObject bodyObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Halcyonite/HalcyoniteBody.prefab").WaitForCompletion();
            var allStateMachines = bodyObject.GetComponents<EntityStateMachine>();
            SetStateOnHurt ssoh = bodyObject.GetComponent<SetStateOnHurt>();
            ssoh.idleStateMachine = allStateMachines.Where(esm => esm != ssoh.targetStateMachine).ToArray();
        }
    }
}
