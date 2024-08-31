using BepInEx.Configuration;
using EntityStates;
using RiskyTweaks.FireSelect;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RiskyTweaks.Tweaks.Survivors.Bandit2.PrimaryAutoFire.FireMode;

namespace RiskyTweaks.Tweaks.Survivors.Bandit2
{
    public class PrimaryAutoFire : TweakBase<PrimaryAutoFire>
    {
        public override string ConfigCategoryString => "Survivors - Bandit";

        public override string ConfigOptionName => "(Client-Side) Primary Autofire";

        public override string ConfigDescriptionString => "Primaries automatically fire when holding down the button.";

        protected override void ReadConfig(ConfigFile config)
        {
            base.ReadConfig(config);
            FireMode.Init(config);

        }

        protected override void ApplyChanges()
        {
            ReloadSkillDef shotgun = Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/FireShotgun2.asset").WaitForCompletion();
            shotgun.mustKeyPress = false;
            shotgun.interruptPriority = EntityStates.InterruptPriority.Skill;

            ReloadSkillDef rifle = Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/Bandit2Blast.asset").WaitForCompletion();
            rifle.mustKeyPress = false;
            rifle.interruptPriority = EntityStates.InterruptPriority.Skill;


            On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.GetMinimumInterruptPriority += Bandit2FirePrimaryBase_GetMinimumInterruptPriority;
            On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.OnEnter += Bandit2FirePrimaryBase_OnEnter;
        }

        private void Bandit2FirePrimaryBase_OnEnter(On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.orig_OnEnter orig, EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase self)
        {
            if (FireMode.currentfireMode == FireMode.Bandit2FireMode.Default)
            {
                self.minimumBaseDuration = 0.3f;
            }
            else
            {
                self.minimumBaseDuration = 0.12f;
            }
            orig(self);
        }

        private InterruptPriority Bandit2FirePrimaryBase_GetMinimumInterruptPriority(On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.orig_GetMinimumInterruptPriority orig, EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase self)
        {
            if (self.fixedAge <= self.minimumDuration && self.inputBank.skill1.wasDown)
            {
                return InterruptPriority.PrioritySkill;
            }
            return InterruptPriority.Any;
        }

        public static class FireMode
        {
            private static bool initialized = false;
            public enum Bandit2FireMode { Default, Spam }
            public static Bandit2FireMode currentfireMode = Bandit2FireMode.Default;
            public static ConfigEntry<bool> enabled;
            public static ConfigEntry<KeyboardShortcut> defaultButton;
            public static ConfigEntry<KeyboardShortcut> spamButton;
            public static List<SkillDef> targetSkills = new List<SkillDef>
            {
                Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/FireShotgun2.asset").WaitForCompletion(),
                Addressables.LoadAssetAsync<ReloadSkillDef>("RoR2/Base/Bandit2/Bandit2Blast.asset").WaitForCompletion()
            };

            private static BodyIndex targetBodyIndex;

            internal static void Init(ConfigFile config)
            {
                if (initialized) return;
                initialized = true;
                ReadConfig(config);

                RoR2Application.onLoad += OnLoad;
                On.RoR2.UI.SkillIcon.Update += SkillIcon_Update;
                FireSelectManager.FireModeActions += FireModeAction;
            }

            private static void ReadConfig(ConfigFile config)
            {
                enabled = config.Bind<bool>("Fire Select - Bandit Primary Autofire", "Use Fire Select", true, "Enable firemode selection. Requires Bandit Primary Autofire to be enabled.");
                enabled.SettingChanged += Enabled_SettingChanged;
                
                defaultButton = config.Bind<KeyboardShortcut>("Fire Select - Bandit Primary Autofire", "Default Button", KeyboardShortcut.Empty, "Button to select Default firemode.");
                spamButton = config.Bind<KeyboardShortcut>("Fire Select - Bandit Primary Autofire", "Spam Button", KeyboardShortcut.Empty, "Button to select Spam firemode.");
            }

            private static void Enabled_SettingChanged(object sender, EventArgs e)
            {
                if (!enabled.Value) currentfireMode = Bandit2FireMode.Default;
            }

            private static void OnLoad()
            {
                targetBodyIndex = BodyCatalog.FindBodyIndex("Bandit2Body");
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
                        self.stockText.SetText(currentfireMode.ToString() + "(" + self.targetSkill.stock + ")");
                    }
                }
            }

            private static void CycleFireMode()
            {
                if (currentfireMode == Bandit2FireMode.Default)
                {
                    currentfireMode = Bandit2FireMode.Spam;
                }
                else
                {
                    currentfireMode = Bandit2FireMode.Default;
                }
            }

            private static void FireModeAction()
            {
                if (!enabled.Value) return;
                float scroll = Input.mouseScrollDelta.y;
                bool nextDown = FireSelectManager.nextButton.Value.IsDown();
                bool prevDown = FireSelectManager.prevButton.Value.IsDown();

                if ((FireSelectManager.scrollSelection.Value && scroll != 0) || (nextDown || prevDown))
                {
                    CycleFireMode();
                }

                if (SneedUtils.GetKeyPressed(defaultButton))
                {
                    currentfireMode = Bandit2FireMode.Default;
                }
                else if (SneedUtils.GetKeyPressed(spamButton))
                {
                    currentfireMode = Bandit2FireMode.Spam;
                }
            }
        }
    }
}
