using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Items
{
    public class GasolineScaling : TweakBase<GasolineScaling>
    {
        public override string ConfigCategoryString => "Items";

        public override string ConfigOptionName => "(Server-Side) Gasoline - Faster Burn Damage";

        public override string ConfigDescriptionString => "Burn damage ticks faster at higher stacks.";

        protected override void ApplyChanges()
        {
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
        }

        private void GlobalEventManager_ProcIgniteOnKill(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(x => x.MatchStfld(typeof(RoR2.InflictDotInfo), "damageMultiplier")))
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<float, int, float>>((damageMult, itemCount) =>
                {
                    damageMult = 0.75f + 0.25f * itemCount;
                    return damageMult;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: GasolineScaling IL Hook failed");
            }
        }
    }
}
