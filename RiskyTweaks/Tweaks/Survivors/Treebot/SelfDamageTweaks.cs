using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Treebot
{
    public class SelfDamageTweaks : TweakBase<SelfDamageTweaks>
    {
        public override string ConfigCategoryString => "Survivors - REX";

        public override string ConfigOptionName => "(Server-Side) Seed Barrage Self-Damage Consistency";

        public override string ConfigDescriptionString => "Seed Barrage self-damage is affected by armor, like the other 2 plant skills.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.EntityStates.Treebot.Weapon.FireMortar2.OnEnter += FireMortar2_OnEnter;
        }

        private void FireMortar2_OnEnter(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(
                 x => x.MatchCallvirt<HealthComponent>("TakeDamage")
                ))
            {
                c.EmitDelegate<Func<DamageInfo, DamageInfo>>((damageInfo) =>
                {
                    damageInfo.damageType &= ~DamageType.BypassArmor;
                    return damageInfo;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: TreeBot SelfDamageTweaks IL Hook failed");
            }
        }
    }
}
