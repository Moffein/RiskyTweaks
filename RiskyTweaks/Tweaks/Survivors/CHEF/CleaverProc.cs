using MonoMod.Cil;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.CHEF
{
    public class CleaverProc : TweakBase<CleaverProc>
    {
        public override string ConfigCategoryString => "Survivors - CHEF";

        public override string ConfigOptionName => "(Server-Side) Dice Proc Coefficient";

        public override string ConfigDescriptionString => "Gives Dice a 1.0 proc coefficient.";

        protected override void ApplyChanges()
        {
            GameObject projectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/Chef/ChefCleaver.prefab").WaitForCompletion();
            ProjectileController pc = projectilePrefab.GetComponent<ProjectileController>();
            pc.procCoefficient = 1f;
            IL.RoR2.Projectile.CleaverProjectile.HandleFlyback += CleaverProjectile_HandleFlyback;
        }

        private void CleaverProjectile_HandleFlyback(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchLdcR4(1.5f)))
            {
                c.EmitDelegate<Func<float, float>>(proc => 1f);
            }
            else
            {
                Debug.LogError("RiskyTweaks: CleaverProc IL hook failed.");
            }
        }
    }
}
