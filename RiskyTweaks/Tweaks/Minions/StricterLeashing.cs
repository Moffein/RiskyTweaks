using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Minions
{
    public class StricterLeashing : TweakBase<StricterLeashing>
    {
        public override string ConfigCategoryString => "Minions";

        public override string ConfigOptionName => "(Server-Side) Stricter Leashing";

        public override string ConfigDescriptionString => "Drones and allies are teleported to the player more frequently.";

        public override bool StopLoadOnConfigDisable => true;

        private const float leashDistSq = 90f * 90f;

        protected override void ApplyChanges()
        {
            IL.RoR2.Items.MinionLeashBodyBehavior.FixedUpdate += MinionLeashBodyBehavior_FixedUpdate;
        }

        private void MinionLeashBodyBehavior_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            bool error = true;
            if (c.TryGotoNext(
                 x => x.MatchLdcR4(RoR2.Items.MinionLeashBodyBehavior.leashDistSq)
                ))
            {
                c.Next.Operand = leashDistSq;
                if (c.TryGotoNext(
                 x => x.MatchLdcR4(RoR2.Items.MinionLeashBodyBehavior.leashDistSq)
                ))
                {
                    c.Next.Operand = leashDistSq;
                    error = false;
                }
            }

            if (error)
            {
                UnityEngine.Debug.LogError("RiskyTweaks: StricterLeashing IL Hook failed");
            }
        }
    }
}
