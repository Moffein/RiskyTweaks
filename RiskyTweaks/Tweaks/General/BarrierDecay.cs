using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.General
{
    public class BarrierDecay : TweakBase
    {
        public override string ConfigCategoryString => "General";

        public override string ConfigOptionName => "(Server-Side) Barrier Decay Scaling";

        public override string ConfigDescriptionString => "Barrier decays slower at low amounts.";

        public override bool StopLoadOnConfigDisable => true;

        public const float minDecay = 0.25f;
        public const float maxDecay = 1f;

        protected override void ApplyChanges()
        {
            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        private void HealthComponent_ServerFixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                 x => x.MatchCallvirt<CharacterBody>("get_barrierDecayRate")
                ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, HealthComponent, float>>((decayRate, self) =>
                {
                    float barrierPercent = self.barrier / self.fullCombinedHealth;
                    return decayRate * Mathf.Lerp(minDecay, maxDecay, barrierPercent);
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: BarrierDecay IL Hook failed");
            }
        }
    }
}
