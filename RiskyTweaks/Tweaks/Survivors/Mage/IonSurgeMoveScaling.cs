using BepInEx.Configuration;
using IL.RoR2.Networking;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Mage
{
    public class IonSurgeMoveScaling : TweakBase<IonSurgeMoveScaling>
    {
        public override string ConfigCategoryString => "Survivors - Artificer";

        public override string ConfigOptionName => "Ion Surge - Disable Movement Speed Scaling";

        public override string ConfigDescriptionString => "Makes Ion Surge always go a consistent distance regardless of movement speed.";

        public override bool StopLoadOnConfigDisable => false;

        protected override void ApplyChanges()
        {
            IL.EntityStates.Mage.FlyUpState.HandleMovements += FlyUpState_HandleMovements;
        }

        private void FlyUpState_HandleMovements(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdfld<EntityStates.BaseState>("moveSpeedStat")
                ))
            {
                c.Index++;
                c.EmitDelegate<Func<float, float>>(orig =>
                {
                    return Enabled.Value ? 10.15f : orig;//10.15 = 7 * 1.45
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: IonSurgeMoveScaling IL hook failed.");
            }
        }
    }
}
