using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Mage
{
    public class FasterIceWall : TweakBase<FasterIceWall>
    {
        public override string ConfigCategoryString => "Survivors - Artificer";

        public override string ConfigOptionName => "(Client-Side) Snapfreeze - Instant Deploy";

        public override string ConfigDescriptionString => "Snapfreeze fires as soon as you let go of the button.";

        protected override void ApplyChanges()
        {
            IL.EntityStates.Mage.Weapon.PrepWall.FixedUpdate += PrepWall_FixedUpdate;
        }

        private void PrepWall_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchLdfld(typeof(EntityStates.Mage.Weapon.PrepWall), "duration")))
            {
                c.EmitDelegate<Func<float, float>>(duration => 0f);
            }
            else
            {
                Debug.LogError("RiskyTweaks: FasterIceWall IL hook failed.");
            }
        }
    }
}
