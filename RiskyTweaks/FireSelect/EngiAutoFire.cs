using BepInEx.Configuration;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiskyTweaks.FireSelect
{
    public static class EngiAutoFire
    {
        //Default firemode is only used when fire select is disabled, because it has no niche when Auto/Hold are a thing.
        public enum EngiFireMode { Default, Auto, Hold }
        public static EngiFireMode currentfireMode = EngiFireMode.Default;
        public static ConfigEntry<bool> Enabled;
        public static ConfigEntry<KeyboardShortcut> holdButton;
        public static ConfigEntry<KeyboardShortcut> autoButton;
        public static List<SkillDef> targetSkills = new List<SkillDef>
        {
            Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyFireGrenade.asset").WaitForCompletion()
        };
        private static BodyIndex targetBodyIndex;

        internal static void Init(ConfigFile config)
        {
            ReadConfig(config);
            RoR2Application.onLoad += OnLoad;
            On.RoR2.UI.SkillIcon.Update += SkillIcon_Update;
            FireSelectManager.FireModeActions += FireModeAction;

            IL.EntityStates.Engi.EngiWeapon.ChargeGrenades.OnEnter += ChargeGrenades_OnEnter;
        }

        private static void ChargeGrenades_OnEnter(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld(typeof(EntityStates.Engi.EngiWeapon.ChargeGrenades), "baseTotalDuration")))
            {
                c.EmitDelegate<Func<float, float>>(duration =>
                {
                    if (currentfireMode == EngiFireMode.Auto)
                    {
                        return EntityStates.Engi.EngiWeapon.ChargeGrenades.baseMaxChargeTime;
                    }
                    else if (currentfireMode == EngiFireMode.Hold)
                    {
                        return 10000000f;
                    }
                    return duration;
                });
            }
            else
            {
                Debug.LogError("RiskyTweaks: EngiAutoFire IL Hook failed.");
            }
        }

        private static void ReadConfig(ConfigFile config)
        {
            Enabled = config.Bind<bool>("Fire Select - Engineer", "Use Fire Select", true, "Enable firemode selection.");
            Enabled.SettingChanged += Enabled_SettingChanged;

            if (Enabled.Value) currentfireMode = EngiFireMode.Hold;

            autoButton = config.Bind<KeyboardShortcut>("Fire Select - Engineer", "Auto Button", KeyboardShortcut.Empty, "Button to select Auto firemode.");
            holdButton = config.Bind<KeyboardShortcut>("Fire Select - Engineer", "Hold Button", KeyboardShortcut.Empty, "Button to select Hold firemode.");
        }

        private static void Enabled_SettingChanged(object sender, System.EventArgs e)
        {
            if (!Enabled.Value)
            {
                currentfireMode = EngiFireMode.Default;
            }
            else
            {
                if (currentfireMode == EngiFireMode.Default) currentfireMode = EngiFireMode.Hold;
            }
        }

        private static void SkillIcon_Update(On.RoR2.UI.SkillIcon.orig_Update orig, RoR2.UI.SkillIcon self)
        {
            orig(self);
            if (Enabled.Value && self.targetSkill && self.targetSkillSlot == SkillSlot.Primary)
            {
                if (self.targetSkill.characterBody.bodyIndex == targetBodyIndex
                && (targetSkills.Contains(self.targetSkill.skillDef)))
                {
                    self.stockText.gameObject.SetActive(true);
                    self.stockText.fontSize = 12f;
                    self.stockText.SetText(currentfireMode.ToString());
                }
            }
        }

        private static void CycleFireMode()
        {
            if (currentfireMode == EngiFireMode.Auto)
            {
                currentfireMode = EngiFireMode.Hold;
            }
            else
            {
                currentfireMode = EngiFireMode.Auto;
            }
        }

        private static void FireModeAction()
        {
            if (!Enabled.Value) return;
            float scroll = Input.mouseScrollDelta.y;
            bool nextDown = FireSelectManager.nextButton.Value.IsDown();
            bool prevDown = FireSelectManager.prevButton.Value.IsDown();

            if (nextDown || prevDown || scroll != 0f)
            {
                CycleFireMode();
            }

            if (SneedUtils.GetKeyPressed(autoButton))
            {
                currentfireMode = EngiFireMode.Auto;
            }
            else if (SneedUtils.GetKeyPressed(holdButton))
            {
                currentfireMode = EngiFireMode.Hold;
            }
        }

        private static void OnLoad()
        {
            targetBodyIndex = BodyCatalog.FindBodyIndex("EngiBody");
        }
    }
}
