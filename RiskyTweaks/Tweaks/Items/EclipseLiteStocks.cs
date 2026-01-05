using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Items
{
    public class EclipseLiteStocks : TweakBase<EclipseLiteStocks>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Eclipse Lite - Scale to Stocks Per Cooldown";

        public override string ConfigDescriptionString => "Eclipse Lite barrier regen is divided by the amount of stocks given per recharge.";

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
            IL.RoR2.CharacterBody.OnSkillCooldown += CharacterBody_OnSkillCooldown;
        }
        private void CharacterBody_OnSkillCooldown(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(x => x.MatchLdsfld(typeof(DLC3Content.Items), "BarrierOnCooldown"))
                && c.TryGotoNext(x => x.MatchCallvirt<HealthComponent>("AddBarrierAuthority")))
            {
                c.Emit(OpCodes.Ldarg_1);//GenericSkill
                c.Emit(OpCodes.Ldarg_2);//Restock count
                c.EmitDelegate<Func<float, GenericSkill, int, float>>((perc, skill, restock) =>
                {
                    if (skill && skill.skillDef)
                    {
                        int rechargeStock = skill.skillDef.GetRechargeStock(skill);
                        if (rechargeStock > 1)
                        {
                            perc /= rechargeStock;
                        }
                    }
                    return perc;
                });
            }
            else
            {
                Debug.LogError("RiskyTweaks: EclipseLiteStocks IL hook failed.");
            }
        }
    }
}
