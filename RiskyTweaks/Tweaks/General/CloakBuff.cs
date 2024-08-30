using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.General
{
    public class CloakBuff : TweakBase<CloakBuff>
    {
        public override string ConfigCategoryString => "General";

        public override string ConfigOptionName => "(Server-Side) Cloak Buff";

        public override string ConfigDescriptionString => "Increases Cloak position update delay from 2s to 3s.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.RoR2.CharacterAI.BaseAI.Target.GetBullseyePosition += Target_GetBullseyePosition;
        }

        private void Target_GetBullseyePosition(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdcR4(2f)
                ))
            {
                c.Next.Operand = 3f;
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: CloakBuff IL Hook failed");
            }
        }
    }
}
