using BepInEx.Configuration;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RiskyTweaks.FireSelect.CaptainAutoFire;

namespace RiskyTweaks.FireSelect
{
    public static class CaptainAutoFire
    {
        public enum CaptainFireMode { Default, Auto, Charged }
        public static CaptainFireMode currentfireMode = CaptainFireMode.Default;
        public static ConfigEntry<bool> enabled;
        public static ConfigEntry<KeyboardShortcut> defaultButton;
        public static ConfigEntry<KeyboardShortcut> chargedButton;
        public static ConfigEntry<KeyboardShortcut> autoButton;
        public static List<SkillDef> targetSkills = new List<SkillDef>
        {
            Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Captain/CaptainShotgun.asset").WaitForCompletion()
        };
        private static BodyIndex targetBodyIndex;

        internal static void Init(ConfigFile config)
        {
            ReadConfig(config);
            RoR2Application.onLoad += OnLoad;
            On.RoR2.UI.SkillIcon.Update += SkillIcon_Update;
            FireSelectManager.FireModeActions += FireModeAction;
            IL.EntityStates.Captain.Weapon.ChargeCaptainShotgun.FixedUpdate += ChargeCaptainShotgun_FixedUpdate;
        }

        private static void ChargeCaptainShotgun_FixedUpdate(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After, x => x.MatchLdfld(typeof(RoR2.InputBankTest.ButtonState), "down")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, EntityStates.Captain.Weapon.ChargeCaptainShotgun, bool>>((down, self) =>
                {
                    if (currentfireMode == CaptainFireMode.Auto) return false;
                    else if (currentfireMode == CaptainFireMode.Charged)
                    {
                        return self.fixedAge < self.chargeDuration;
                    }

                    return down;
                });
            }
            else
            {
                Debug.LogError("RiskyTweaks: CaptainAutoFire IL Hook failed.");
            }
        }

        public static void CycleFireMode(int direction)
        {
            int newFireMode = direction + (int)currentfireMode;
            if (newFireMode < 0)
            {
                currentfireMode = CaptainFireMode.Charged;
            }
            else if (newFireMode > 2)
            {
                currentfireMode = CaptainFireMode.Default;
            }
            else
            {
                currentfireMode = (CaptainFireMode)newFireMode;
            }
        }

        public static void FireModeAction()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (FireSelectManager.scrollSelection.Value && scroll != 0)
            {
                CycleFireMode(scroll < 0 ? -1 : 1);
            }
            if (SneedUtils.GetKeyPressed(FireSelectManager.nextButton))
            {
                CycleFireMode(1);
            }
            if (SneedUtils.GetKeyPressed(FireSelectManager.prevButton))
            {
                CycleFireMode(-1);
            }

            if (SneedUtils.GetKeyPressed(defaultButton))
            {
                currentfireMode = CaptainFireMode.Default;
            }
            if (SneedUtils.GetKeyPressed(autoButton))
            {
                currentfireMode = CaptainFireMode.Auto;
            }
            if (SneedUtils.GetKeyPressed(chargedButton))
            {
                currentfireMode = CaptainFireMode.Charged;
            }
        }

        private static void ReadConfig(ConfigFile config)
        {
            enabled = config.Bind<bool>("Fire Select - Captain", "Use Fire Select", true, "Enable firemode selection.");
            enabled.SettingChanged += Enabled_SettingChanged;

            defaultButton = config.Bind<KeyboardShortcut>("Fire Select - Captain", "Default Button", KeyboardShortcut.Empty, "Button to select Default firemode.");
            autoButton = config.Bind<KeyboardShortcut>("Fire Select - Captain", "Auto Button", KeyboardShortcut.Empty, "Button to select Auto firemode.");
            chargedButton = config.Bind<KeyboardShortcut>("Fire Select - Captain", "Charged Button", KeyboardShortcut.Empty, "Button to select Charged firemode.");
        }

        private static void Enabled_SettingChanged(object sender, EventArgs e)
        {
            if (!enabled.Value) currentfireMode = CaptainFireMode.Default;
        }

        private static void SkillIcon_Update(On.RoR2.UI.SkillIcon.orig_Update orig, RoR2.UI.SkillIcon self)
        {
            orig(self);
            if (enabled.Value && self.targetSkill && self.targetSkillSlot == SkillSlot.Primary)
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

        private static void OnLoad()
        {
            targetBodyIndex = BodyCatalog.FindBodyIndex("CaptainBody");
        }
    }
}
