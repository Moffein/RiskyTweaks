using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Enemies
{
    public class RaiseLevelCap : TweakBase<RaiseLevelCap>
    {
        public override string ConfigCategoryString => "Enemies";

        public override string ConfigOptionName => "(Server-Side) Raise Level Cap";

        public override string ConfigDescriptionString => "Raise monster level cap.";

        public override bool StopLoadOnConfigDisable => true;


        public static float levelCap = 9999;
        public static float stopSound = 300f; //stop levelup sounds after this threshold to prevent spam

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            levelCap = config.Bind<float>(ConfigCategoryString, ConfigOptionName + " - Max Level", 9999f, "Maximum level if Raise Level Cap is enabled.").Value;
        }

        protected override void ApplyChanges()
        {
            IL.RoR2.Run.RecalculateDifficultyCoefficentInternal += Run_RecalculateDifficultyCoefficentInternal;
            On.RoR2.LevelUpEffectManager.OnRunAmbientLevelUp += LevelUpEffectManager_OnRunAmbientLevelUp;
        }

        private void LevelUpEffectManager_OnRunAmbientLevelUp(On.RoR2.LevelUpEffectManager.orig_OnRunAmbientLevelUp orig, Run run)
        {
            if (run.ambientLevel <= stopSound) orig(run);
        }

        private void Run_RecalculateDifficultyCoefficentInternal(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                x => x.MatchLdsfld<Run>(nameof(Run.ambientLevelCap))
                ))
            {
                c.Remove();
                c.Emit<RaiseLevelCap>(OpCodes.Ldsfld, nameof(RaiseLevelCap.levelCap));
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: RaiseLevelCap IL Hook failed");
            }
        }
    }
}
