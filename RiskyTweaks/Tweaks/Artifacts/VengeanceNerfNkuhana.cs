using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Artifacts
{
    public class VengeanceNerfNkuhana : TweakBase<VengeanceNerfNkuhana>
    {
        public override string ConfigCategoryString => "Artifacts - Vengeance";

        public override string ConfigOptionName => "(Server-Side) Nerf Nkuhanas Opinion";

        public override string ConfigDescriptionString => "Nkuhanas Opinion deals 90% less damage from Vengeance Umbras.";

        protected override void ApplyChanges()
        {
            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        private void HealthComponent_ServerFixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchStfld<RoR2.Orbs.DevilOrb>("damageValue")
                ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, HealthComponent, float>>((damage, self) =>
                {
                    if (self.itemCounts.invadingDoppelganger > 0)
                    {
                        damage *= 0.1f;
                    }
                    return damage;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: VengeanceNerfNkuhana IL Hook failed");
            }
        }
    }
}
