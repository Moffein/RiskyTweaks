using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Croco
{
    public class PoisonDamageCap : TweakBase<PoisonDamageCap>
    {
        public override string ConfigCategoryString => "Survivors - Acrid";

        public override string ConfigOptionName => "(Server-Side) Remove Poison Damage Cap";

        public override string ConfigDescriptionString => "Removes the hidden damage cap of Poison";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.RoR2.DotController.AddDot += DotController_AddDot;
        }

        private void DotController_AddDot(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdcR4(50f)
                ))
            {
                c.Index += 2;
                c.EmitDelegate<Func<float, float>>(orig =>
                {
                    return float.MaxValue;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: Croco PoisonDamageCap IL Hook failed");
            }
        }
    }
}
