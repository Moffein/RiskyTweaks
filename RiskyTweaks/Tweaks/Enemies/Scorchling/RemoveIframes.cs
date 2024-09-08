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
            IL.ScorchlingController.Burrow += CommonHooks.DisableAddBuffGeneric;
            IL.ScorchlingController.Breach += CommonHooks.DisableRemoveBuffGeneric;
        }
    }
}
