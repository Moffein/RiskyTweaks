using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2;
using RoR2.Projectile;

namespace RiskyTweaks.Tweaks.Survivors.Seeker
{
    public class SpiritPunchProcCoefficient : TweakBase<SpiritPunchProcCoefficient>
    {
        public override string ConfigCategoryString => "Survivors - Seeker";

        public override string ConfigOptionName => "(Server-Side) Spirit Punch Proc Coefficient";

        public override string ConfigDescriptionString => "Fixes Spirit Punch 3rd hit having a lower proc coefficient.";

        protected override void ApplyChanges()
        {
            GameObject projectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Seeker/SpiritPunchFinisherProjectile.prefab").WaitForCompletion();
            ProjectileImpactExplosion pie = projectilePrefab.GetComponent<ProjectileImpactExplosion>();
            pie.blastProcCoefficient = 1f;
        }
    }
}
