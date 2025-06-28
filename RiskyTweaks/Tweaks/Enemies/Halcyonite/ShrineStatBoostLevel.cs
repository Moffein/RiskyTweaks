using UnityEngine;

namespace RiskyTweaks.Tweaks.Enemies.Halcyonite
{
    public class ShrineStatBoostLevel : TweakBase<ShrineStatBoostLevel>
    {
        public override string ConfigCategoryString => "Enemies - Halcyonite";

        public override string ConfigOptionName => "(Server-Side) Shrine Stat Boost Cap";

        public override string ConfigDescriptionString => "Caps shrine stat boosts at +20 (12x damage, 4x hp).";

        protected override void ApplyChanges()
        {
            On.RoR2.CombatDirector.HalcyoniteShrineActivation += CombatDirector_HalcyoniteShrineActivation;
        }

        private void CombatDirector_HalcyoniteShrineActivation(On.RoR2.CombatDirector.orig_HalcyoniteShrineActivation orig, RoR2.CombatDirector self, float monsterCredit, RoR2.DirectorCard chosenDirectorCard, int difficultyLevel, UnityEngine.Transform shrineTransform)
        {
            orig(self, monsterCredit, chosenDirectorCard, Mathf.Min(20, difficultyLevel), shrineTransform);
        }
    }
}
