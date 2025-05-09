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

        //CURSED
        private BodyIndex seekerBodyIndex;
        private List<HealthComponent> seekerReprieveHealthComponents = new List<HealthComponent>();

        protected override void ApplyChanges()
        {
            //Cursed code to make Reprieve not last forever
            On.EntityStates.Seeker.Reprieve.OnEnter += Reprieve_OnEnter;
            On.EntityStates.Seeker.Reprieve.OnExit += Reprieve_OnExit;
            RoR2.Stage.onStageStartGlobal += Stage_onStageStartGlobal;

            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            seekerReprieveHealthComponents.Clear();
        }

        private void Reprieve_OnExit(On.EntityStates.Seeker.Reprieve.orig_OnExit orig, EntityStates.Seeker.Reprieve self)
        {
            if (self.healthComponent) seekerReprieveHealthComponents.Remove(self.healthComponent);
            orig(self);
        }

        private void Reprieve_OnEnter(On.EntityStates.Seeker.Reprieve.orig_OnEnter orig, EntityStates.Seeker.Reprieve self)
        {
            orig(self);
            if (self.healthComponent) seekerReprieveHealthComponents.Add(self.healthComponent);
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
                    if (seekerReprieveHealthComponents.Contains(self)) return decayRate;

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
