using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2.Projectile;
using RoR2;

namespace RiskyTweaks.Tweaks.Survivors.Toolbot
{
    public class ScrapLauncherFalloff : TweakBase<ScrapLauncherFalloff>
    {
        public override string ConfigCategoryString => "Survivors - MUL-T";

        public override string ConfigOptionName => "(Server-Side) Scrap Launcher - Remove Falloff";

        public override string ConfigDescriptionString => "Scrap Launcher always deals full damage to enemies in the AoE.";

        protected override void ApplyChanges()
        {
            GameObject projectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeLauncherProjectile.prefab").WaitForCompletion();
            ProjectileImpactExplosion pie = projectile.GetComponent<ProjectileImpactExplosion>();
            pie.falloffModel = BlastAttack.FalloffModel.None;
        }
    }
}
