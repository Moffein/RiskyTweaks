using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Croco
{
    public class Regenerative : TweakBase<Regenerative>
    {
        public override string ConfigCategoryString => "Survivors - Acrid";

        public override string ConfigOptionName => "(Server-Side) Regenerative is Healing";

        public override string ConfigDescriptionString => "Regenerative counts as healing and is no longer affected by difficulty regen modifiers.";

        protected override void ApplyChanges()
        {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        private void CharacterBody_RecalculateStats(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "CrocoRegen")))
            {
                c.EmitDelegate<Func<BuffDef, BuffDef>>(orig => null);
            }
            else
            {
                Debug.LogError("RiskyTweaks: Croco Regenerative IL hook failed.");
            }
        }

        private void HealthComponent_ServerFixedUpdate(On.RoR2.HealthComponent.orig_ServerFixedUpdate orig, RoR2.HealthComponent self, float deltaTime)
        {
            orig(self, deltaTime);
            if (self.body && self.body.HasBuff(RoR2Content.Buffs.CrocoRegen))
            {
                self.HealFraction(0.1f * deltaTime, default);
            }
        }
    }
}
