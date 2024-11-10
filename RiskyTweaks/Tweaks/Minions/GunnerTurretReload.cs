using RoR2.Skills;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Minions
{
    public class GunnerTurretReload : TweakBase<GunnerTurretReload>
    {
        public override string ConfigCategoryString => "Minions";

        public override string ConfigOptionName => "(Server-Side) Gunner Turret - Remove Reload";

        public override string ConfigDescriptionString => "Gunner Turrets no longer stop to reload.";

        protected override void ApplyChanges()
        {
            SkillDef turretSkill = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Drones/Turret1BodyTurret.asset").WaitForCompletion();
            turretSkill.baseMaxStock = 1;
            turretSkill.baseRechargeInterval = 0f;
        }
    }
}
