using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies.LunarWisp
{
    public class HitscanFalloff : TweakBase<HitscanFalloff>
    {
        public override string ConfigCategoryString => "Enemies - Lunar Wisp";

        public override string ConfigOptionName => "(Server-Side) Bullet Falloff";

        public override string ConfigDescriptionString => "Hitscan attacks have falloff.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.EntityStates.LunarWisp.FireLunarGuns.OnFireAuthority += FireLunarGuns_OnFireAuthority;
        }

        private void FireLunarGuns_OnFireAuthority(ILContext il)
        {
            bool error = true;

            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchCallvirt<BulletAttack>("Fire")
                ))
            {
                c.EmitDelegate<Func<BulletAttack, BulletAttack>>(bulletAttack =>
                {
                    bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                    return bulletAttack;
                });

                if (c.TryGotoNext(
                 x => x.MatchCallvirt<BulletAttack>("Fire")
                ))
                {
                    c.EmitDelegate<Func<BulletAttack, BulletAttack>>(bulletAttack =>
                    {
                        bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                        return bulletAttack;
                    });
                    error = false;
                }
            }

            if (error)
            {
                UnityEngine.Debug.LogError("RiskyTweaks: LunarWisp HitscanFalloff IL Hook failed");
            }
        }
    }
}
