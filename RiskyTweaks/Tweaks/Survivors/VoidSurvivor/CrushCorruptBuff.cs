using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.VoidSurvivor
{
    public class CrushCorruptBuff : TweakBase<CrushCorruptBuff>
    {
        public override string ConfigCategoryString => "Survivors - Void Fiend";

        public override string ConfigOptionName => "(Client-Side) Corrupted Crush - Buff Skill";

        public override string ConfigDescriptionString => "Corrupted Crush triggers faster and can be retriggered as many times as you want.";

        protected override void ApplyChanges()
        {
            SkillDef crushHealth = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/CrushHealth.asset").WaitForCompletion();
            crushHealth.baseMaxStock = 1;
            crushHealth.baseRechargeInterval = 0;
            crushHealth.rechargeStock = 1;

            SneedUtils.SetAddressableEntityStateField("RoR2/DLC1/VoidSurvivor/EntityStates.VoidSurvivor.Weapon.ChargeCrushHealth.asset", "baseDuration", "0.3"); //vanilla is 1
        }
    }
}
