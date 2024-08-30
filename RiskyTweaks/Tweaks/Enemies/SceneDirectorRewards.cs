using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class SceneDirectorRewards : TweakBase<SceneDirectorRewards>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) Scenedirector Monster Rewards";

        public override string ConfigDescriptionString => "Monsters that spawn on stage start give the same amount of money as Teleporter monsters.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.RoR2.SceneDirector.PopulateScene += SceneDirector_PopulateScene;
        }

        private void SceneDirector_PopulateScene(On.RoR2.SceneDirector.orig_PopulateScene orig, RoR2.SceneDirector self)
        {
            //Enemies spawned with the level are worth the same as Teleporter enemies.
            if (self && self.expRewardCoefficient < 0.11f)
            {
                self.expRewardCoefficient = 0.11f;
            }
            orig(self);
        }
    }
}
