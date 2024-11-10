using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Survivors.Seeker
{
    public class RemoveM2FoV : TweakBase<RemoveM2FoV>
    {
        public override string ConfigCategoryString => "Survivors - Seeker";

        public override string ConfigOptionName => "(Client-Side) Remove M2 FoV Modifier";

        public override string ConfigDescriptionString => "Prevents FoV from changing when using Seeker's M2.";

        public override bool StopLoadOnConfigDisable => false;

        protected override void ApplyChanges()
        {
            IL.EntityStates.Seeker.UnseenHand.FixedUpdate += UnseenHand_FixedUpdate;
        }

        private void UnseenHand_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchCall(typeof(EntityStates.EntityState), "get_cameraTargetParams")))
            {
                c.EmitDelegate<Func<CameraTargetParams, CameraTargetParams>>(orig => null);
            }
            else
            {
                Debug.LogError("RiskyTweaks: Seeker RemoveM2FoV IL Hook failed.");
            }
        }
    }
}
