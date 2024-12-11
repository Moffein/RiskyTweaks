using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Minions
{
    public class AllyRegen : TweakBase<AllyRegen>
    {
        public override string ConfigCategoryString => "Minions";

        public override string ConfigOptionName => "(Server-Side) Ally Regen Buff";

        public override string ConfigDescriptionString => "Allies regen to full HP faster and in a consistent amount of time.";

        public static Dictionary<string, float> droneInfo = new Dictionary<string, float>
        {
            {"BeetleGuardAllyBody" , 40f },
            {"RoboBallRedBuddyBody" , 40f },
            {"RoboBallGreenBuddyBody" , 40f },
            {"DroneCommanderBody" , 40f },

            {"Drone1Body", 40f },
            {"Drone2Body", 40f },
            {"MissileDroneBody", 40f },
            {"FlameDroneBody", 40f },
            {"EmergencyDroneBody", 40f },
            {"MegaDroneBody", 30f },

            {"CloneDroneBody", 40f },
            {"ShockDroneBody", 40f },
            {"ChillDroneBody", 40f },
            {"ItemDroneBody", 40f },
            {"BulwarkDroneBody", 40f },
            {"ShredderDroneBody", 40f },
            {"BoosterDroneBody", 40f },
            {"HellDroneBody", 40f },


            {"QbDroneBody", 40f },
            {"PsyDroneGreenBody", 40f },
            {"PsyDroneRedBody", 40f },
            {"LaserDrone1Body", 40f },
            {"LaserDrone2Body", 40f },
        };

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
            RoR2Application.onLoad += ChangeStats;
        }

        //Do this after all bodies are loaded
        private void ChangeStats()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod"))
            {
                Debug.LogWarning("RiskyTweaks: Skipping AllyRegen because RiskyMod is loaded.");
                return;
            }

            foreach (var key in droneInfo.Keys)
            {
                SetRegen(key, droneInfo.GetValueOrDefault(key));
            }
        }

        public static void SetRegen(string bodyPrefabName, float fullRegenTime)
        {
            GameObject bodyPrefab = BodyCatalog.FindBodyPrefab(bodyPrefabName);
            if (!bodyPrefab)
            {
                Debug.LogError("AllyRegen.SetRegen: "+bodyPrefabName+" is null");
                return;
            }

            if (fullRegenTime <= 0f)
            {
                Debug.LogError("AllyRegen.SetRegen: fullRegenTime must be greater than 0");
                return;
            }

            CharacterBody cb = bodyPrefab.GetComponent<CharacterBody>();
            if (!cb)
            {
                Debug.LogError("AllyRegen.SetRegen: " + bodyPrefab + " does not have a CharacterBody");
                return;
            }

            cb.baseRegen = cb.baseMaxHealth / fullRegenTime;
            cb.levelRegen = cb.levelMaxHealth / fullRegenTime;
        }
    }
}
