using MonoMod.RuntimeDetour;
using R2API.Utils;
using RoR2.Skills;

namespace RiskyTweaks.Tweaks.Survivors.Captain
{
    internal class OrbitalHiddenRealms : TweakBase<OrbitalHiddenRealms>
    {
        public override string ConfigCategoryString => "Survivors - Captain";

        public override string ConfigOptionName => "(Client-Side) Orbital Skills in Hidden Realms";

        public override string ConfigDescriptionString => "Allows Orbital Skills to be used in Hidden Realms.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            var getIsAvailable = new Hook(typeof(CaptainOrbitalSkillDef).GetMethodCached("get_isAvailable"),
                typeof(OrbitalHiddenRealms).GetMethodCached(nameof(IsAvailable)));
        }

        private static bool IsAvailable(CaptainOrbitalSkillDef self)
        {
            return true;
        }
    }
}
