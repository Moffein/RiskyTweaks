using BepInEx.Configuration;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Survivors.Huntress
{
    public class Tracking : TweakBase<Tracking>
    {
        public override string ConfigCategoryString => "Survivors - Huntress";

        public override string ConfigOptionName => "(Client-Side) Tracking Tweaks";

        public override string ConfigDescriptionString => "Makes Huntress target the enemy closest to your crosshair, and increases the targeting angle.";

        public override bool StopLoadOnConfigDisable => true;

        public ConfigEntry<BullseyeSearch.SortMode> trackingMode;

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            trackingMode = config.Bind<BullseyeSearch.SortMode>(ConfigCategoryString, ConfigOptionName + " - Tracking Mode", BullseyeSearch.SortMode.Angle, "Type of tracking to use. Vanilla is by Distance only.");
        }

        protected override void ApplyChanges()
        {
            On.RoR2.HuntressTracker.Start += HuntressTracker_Start;
            IL.RoR2.HuntressTracker.SearchForTarget += HuntressTracker_SearchForTarget;
        }

        private void HuntressTracker_Start(On.RoR2.HuntressTracker.orig_Start orig, RoR2.HuntressTracker self)
        {
            orig(self);
            self.maxTrackingAngle = 45f;
        }

        private void HuntressTracker_SearchForTarget(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchStfld(typeof(BullseyeSearch), "sortMode")
                ))
            {
                c.EmitDelegate<Func<BullseyeSearch.SortMode, BullseyeSearch.SortMode>>(orig =>
                {
                    return trackingMode.Value;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: Huntress Tracking IL Hook failed");
            }
        }
    }
}
