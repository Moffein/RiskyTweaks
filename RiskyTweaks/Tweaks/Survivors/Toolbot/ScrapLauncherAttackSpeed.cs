using RoR2.Skills;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.Tweaks.Survivors.Toolbot
{
    public class ScrapLauncherAttackSpeed : TweakBase<ScrapLauncherAttackSpeed>
    {
        public override string ConfigCategoryString => "Survivors - MUL-T";

        public override string ConfigOptionName => "(Client-Side) Scrap Launcher - Attack Speed Scaling";

        public override string ConfigDescriptionString => "Scrap Launcher cooldown scales with attack speed.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            ToolbotWeaponSkillDef scrapDef = Addressables.LoadAssetAsync<ToolbotWeaponSkillDef>("RoR2/Base/Toolbot/ToolbotBodyFireGrenadeLauncher.asset").WaitForCompletion();
            scrapDef.attackSpeedBuffsRestockSpeed = true;
            scrapDef.attackSpeedBuffsRestockSpeed_Multiplier = 1f;
        }
    }
}
