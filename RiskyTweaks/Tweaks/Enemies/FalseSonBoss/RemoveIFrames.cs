using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies.FalseSonBoss
{
    public class RemoveIFrames : TweakBase<RemoveIFrames>
    {
        public override string ConfigCategoryString => "Enemies - False Son";

        public override string ConfigOptionName => "(Server-Side) Remove Invulnerability Frames";

        public override string ConfigDescriptionString => "Removes invulnerability from Corrupted Paths.";

        protected override void ApplyChanges()
        {
            IL.EntityStates.FalseSonBoss.CorruptedPathsDash.OnEnter += CommonHooks.DisableAddBuffGeneric;
            IL.EntityStates.FalseSonBoss.CorruptedPathsDash.OnExit += CommonHooks.DisableRemoveBuffGeneric;
        }
    }
}
