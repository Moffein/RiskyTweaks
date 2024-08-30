namespace RiskyTweaks.Tweaks.General
{
    public class ShockBuff : TweakBase<ShockBuff>
    {
        public override string ConfigCategoryString => "General";

        public override string ConfigOptionName => "(Server-Side) Shock Buff";

        public override string ConfigDescriptionString => "Removes Shock HP threshold.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            SneedUtils.SetAddressableEntityStateField("RoR2/Base/Common/EntityStates.ShockState.asset", "healthFractionToForceExit", "1");
        }
    }
}
