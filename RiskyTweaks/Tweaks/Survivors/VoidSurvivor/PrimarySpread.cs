using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.VoidSurvivor
{
    public class PrimarySpread : TweakBase<PrimarySpread>
    {
        public override string ConfigCategoryString => "Survivors - Void Fiend";

        public override string ConfigOptionName => "(Client-Side) Drown - Remove Spread";

        public override string ConfigDescriptionString => "Removes random spread.";

        protected override void ApplyChanges()
        {
            IL.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += RemoveBulletSpread;
            IL.EntityStates.VoidSurvivor.Weapon.FireCorruptHandBeam.FireBullet += RemoveBulletSpread;
        }

        private void RemoveBulletSpread(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchCallvirt<BulletAttack>("Fire")
                 ))
            {
                c.EmitDelegate<Func<BulletAttack, BulletAttack>>(bulletAttack =>
                {
                    bulletAttack.minSpread = 0f;
                    bulletAttack.maxSpread = 0f;
                    return bulletAttack;
                });
            }
            else
            {
                Debug.LogError("RiskyTweaks: VoidSurvivor PrimarySpread IL hook failed.");
            }
        }
    }
}
