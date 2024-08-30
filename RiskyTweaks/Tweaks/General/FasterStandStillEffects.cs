using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.General
{
    public class FasterStandStillEffects : TweakBase<FasterStandStillEffects>
    {
        public override string ConfigCategoryString => "General";

        public override string ConfigOptionName => "(Server-Side) Faster Stand-Still Effects";

        public override string ConfigDescriptionString => "Lowers the duration required to trigger On-Standing-Still effects.";

        protected override void ApplyChanges()
        {
            IL.RoR2.CharacterBody.GetNotMoving += CharacterBody_GetNotMoving;
        }

        private void CharacterBody_GetNotMoving(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdcR4(1f)
                ))
            {
                c.Next.Operand = 0.5f;
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: FasterStandStillEffects IL Hook failed");
            }
        }
    }
}
