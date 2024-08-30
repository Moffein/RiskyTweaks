using MonoMod.Cil;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class FullVision : TweakBase<FullVision>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) AI Full Vision";

        public override string ConfigDescriptionString => "Enemies are always aware of player locations.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.RoR2.CharacterAI.BaseAI.ManagedFixedUpdate += BaseAI_ManagedFixedUpdate;
        }

        private void BaseAI_ManagedFixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                MoveType.After,
                x => x.MatchLdfld(typeof(BaseAI), "fullVision")
                ))
            {
                c.EmitDelegate<Func<bool, bool>>(useFullVision => true);
            }
            else
            {
                Debug.LogError("RiskyTweaks: FullVision IL Hook failed.");
            }
        }
    }
}
