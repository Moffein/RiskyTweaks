using RoR2;

namespace RiskyTweaks.Tweaks.Survivors.Toolbot
{
    public class RetoolReload : TweakBase<RetoolReload>
    {
        public override string ConfigCategoryString => "Survivors - MUL-T";

        public override string ConfigOptionName => "(Client-Side) Retool Reload";

        public override string ConfigDescriptionString => "Retool resets primary stocks to full.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            On.EntityStates.Toolbot.ToolbotStanceSwap.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.isAuthority)
                {
                    GenericSkill skill1 = self.GetPrimarySkill1();
                    if (skill1) skill1.stock = skill1.maxStock;

                    GenericSkill skill2 = self.GetPrimarySkill2();
                    if (skill2) skill2.stock = skill2.maxStock;
                }
            };
        }
    }
}
