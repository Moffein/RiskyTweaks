using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Enemies.Scorchling
{
    public class RemoveIframes : TweakBase<RemoveIframes>
    {
        public override string ConfigCategoryString => "Enemies - Scorch Wurm";

        public override string ConfigOptionName => "(Server-Side) Remove Invulnerability Frames";

        public override string ConfigDescriptionString => "Removes invulnerability when burrowing.";

        protected override void ApplyChanges()
        {
            IL.ScorchlingController.Burrow += ScorchlingController_Burrow;
            IL.ScorchlingController.Breach += ScorchlingController_Breach;
        }

        private void ScorchlingController_Breach(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(x => x.MatchCallvirt(typeof(CharacterBody), "RemoveBuff")))
            {
                c.EmitDelegate<Func<BuffIndex, BuffIndex>>(orig => BuffIndex.None);
            }
            else
            {
                Debug.LogError("RiskyTweaks: Scorchling RemoveIFrames Breach IL Hook failed");
            }
        }

        private void ScorchlingController_Burrow(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(x => x.MatchCallvirt(typeof(CharacterBody), "AddBuff")))
            {
                c.EmitDelegate<Func<BuffIndex, BuffIndex>>(orig => BuffIndex.None);
            }
            else
            {
                Debug.LogError("RiskyTweaks: Scorchling RemoveIFrames Burrow IL Hook failed");
            }
        }
    }
}
