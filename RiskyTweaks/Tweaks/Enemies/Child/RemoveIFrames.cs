using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies.Child
{
    public class RemoveIFrames : TweakBase<RemoveIFrames>
    {
        public override string ConfigCategoryString => "Enemies - Child";

        public override string ConfigOptionName => "(Server-Side) Remove Invulnerability Frames";

        public override string ConfigDescriptionString => "Removes invulnerability when teleporting.";

        protected override void ApplyChanges()
        {
            On.RoR2.ChildMonsterController.RegisterTeleport += ChildMonsterController_RegisterTeleport;
        }

        private void ChildMonsterController_RegisterTeleport(On.RoR2.ChildMonsterController.orig_RegisterTeleport orig, RoR2.ChildMonsterController self, bool addInvincibility)
        {
            orig(self, false);
        }
    }
}
