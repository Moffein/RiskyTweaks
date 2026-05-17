using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Enemies.DLC3
{
    public class SolusArmor : TweakBase<SolusArmor>
    {
        public override string ConfigCategoryString => "Enemies - DLC3";

        public override string ConfigOptionName => "Remove armor from Solus Scorchers.";

        public override string ConfigDescriptionString => "Removes armor from Solus Scorchers.";

        protected override void ApplyChanges()
        {
            Addressables.LoadAssetAsync<GameObject>("RoR2/DLC3/Tanker/TankerBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseArmor = 0f;
        }
    }
}
